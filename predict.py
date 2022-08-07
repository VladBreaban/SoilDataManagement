def get_json(df):
    """ Small function to serialise DataFrame dates as 'YYYY-MM-DD' in JSON """
    import json
    import datetime
    def convert_timestamp(item_date_object):
        if isinstance(item_date_object, (datetime.date, datetime.datetime)):
            return item_date_object.strftime("%Y-%m-%d")

    dict_ = df.to_dict(orient='records')

    return json.dumps(dict_, default=convert_timestamp)


import pandas as pd
import numpy as np

"""LSTM model development"""
from sklearn.preprocessing import MinMaxScaler
import tensorflow as tf
from tensorflow import keras
from keras.models import Sequential
from keras.layers import Dense, Dropout, LSTM

data = pd.read_csv("C:\\Users\\vladu\\Downloads\\20220620cleanData.csv", sep=',')
data = data.rename(columns={'CreatedDate': 'date'})

data['date'] = pd.to_datetime(data['date'], format='%Y.%m.%d')
data = data.set_index('date')
og_df = data
todataframe = og_df.reset_index(inplace=False)

print("\n<----------------------Info of the OG dataset---------------------->")
print(todataframe.info())
print("<-------------------------------------------------------------------->\n")

# dataframe creation
seriesdata = todataframe.sort_index(ascending=True, axis=0)
new_seriesdata = pd.DataFrame(index=range(0, len(todataframe)), columns=['date', 'N'])
for i in range(0, len(seriesdata)):
    new_seriesdata['date'][i] = seriesdata['date'][i]
    new_seriesdata['N'][i] = seriesdata['N'][i]
# setting the index again
new_seriesdata.index = new_seriesdata.date
new_seriesdata.drop('date', axis=1, inplace=True)
# creating train and test sets this comprises the entire data’s present in the dataset
myseriesdataset = new_seriesdata.values
totrain = myseriesdataset
# converting dataset into x0_train and y_train
scalerdata = MinMaxScaler(feature_range=(0, 1))
scale_data = scalerdata.fit_transform(myseriesdataset)
x_totrain, y_totrain = [], []
length_of_totrain = len(totrain)
print(length_of_totrain)
for i in range(1, length_of_totrain):
    x_totrain.append(scale_data[i - 1:i, 0])
    y_totrain.append(scale_data[i, 0])
x_totrain, y_totrain = np.array(x_totrain), np.array(y_totrain)
x_totrain = np.reshape(x_totrain, (x_totrain.shape[0], x_totrain.shape[1], 1))
# LSTM neural network
lstm_model = Sequential()
lstm_model.add(LSTM(units=70, return_sequences=True, input_shape=(x_totrain.shape[1], 1)))
lstm_model.add(LSTM(units=70))
lstm_model.add(Dense(1))
lstm_model.compile(loss='mean_squared_error', optimizer='adam')
lstm_model.fit(x_totrain, y_totrain, epochs=100, batch_size=1, verbose=2)
# predicting next data stock price
print(len(new_seriesdata))
myinputs = new_seriesdata[len(new_seriesdata) - (10) - 1:].values
myinputs = myinputs.reshape(-1, 1)
myinputs = scalerdata.transform(myinputs)
tostore_test_result = []

for i in range(1, myinputs.shape[0]):
    tostore_test_result.append(myinputs[i - 1:i, 0])

tostore_test_result = np.array(tostore_test_result)
tostore_test_result = np.reshape(tostore_test_result, (tostore_test_result.shape[0], tostore_test_result.shape[1], 1))
myclosing_priceresult = lstm_model.predict(tostore_test_result)
myclosing_priceresult = scalerdata.inverse_transform(myclosing_priceresult)

# Combining og and predicted dataset for end result.
datelist = pd.date_range(pd.datetime.now().date(), periods=11)[1:]
predicted_df = pd.DataFrame(myclosing_priceresult, columns=['N'], index=datelist)
result_df = pd.concat([og_df, predicted_df])[['N']]
result_df = result_df.reset_index(inplace=False)
result_df.columns = ['date', 'N']

# to print the info of the END RESULT dataset
print("\n<----------------------Info of the RESULT dataset---------------------->")
print(result_df.info())
print("<------------------------------------------------------------------------>\n")

import matplotlib.pyplot as plt

plt.plot(result_df['N'])

plt.show()

print("\n<----------------------HELLO---------------------->")
print(get_json(result_df))