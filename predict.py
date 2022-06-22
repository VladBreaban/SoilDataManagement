# Data manipulation
# ==============================================================================
import numpy as np
import pandas as pd

# Plots
# ==============================================================================
import matplotlib.pyplot as plt
plt.style.use('fivethirtyeight')
plt.rcParams['lines.linewidth'] = 1.5
%matplotlib inline

# Modeling and Forecasting
# ==============================================================================
from sklearn.linear_model import LinearRegression
from sklearn.linear_model import Lasso
from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import mean_squared_error
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import make_pipeline

from skforecast.ForecasterAutoreg import ForecasterAutoreg
from skforecast.ForecasterAutoregCustom import ForecasterAutoregCustom
from skforecast.ForecasterAutoregMultiOutput import ForecasterAutoregMultiOutput
from skforecast.model_selection import grid_search_forecaster
from skforecast.model_selection import backtesting_forecaster

from joblib import dump, load

# Warnings configuration
# ==============================================================================
import warnings
# warnings.filterwarnings('ignore')


data = pd.read_csv('C:\\Users\\Vlad\\Desktop\\net6\\20220620cleanData.csv', sep=',')
data = data.rename(columns={'CreatedDate': 'date'})

data['date'] = pd.to_datetime(data['date'],format='%Y.%m.%d')
data=data.set_index('date')
data.asfreq(freq='D')
data.head()
(data.index == pd.date_range(start=data.index.min(),
                             end=data.index.max(),
                             freq=data.index.freq)).all()
steps = 4
data_train = data[:-steps]
data_test  = data[-steps:]


print(f"Train dates : {data_train.index.min()} --- {data_train.index.max()}  (n={len(data_train)})")
print(f"Test dates  : {data_test.index.min()} --- {data_test.index.max()}  (n={len(data_test)})")

fig, ax=plt.subplots(figsize=(9, 4))
data_train['N'].plot(ax=ax, label='train')
data_test['N'].plot(ax=ax, label='test')
ax.legend();

forecaster = ForecasterAutoreg(
                regressor = RandomForestRegressor(random_state=123),
                lags = 6
                )




forecaster.fit(data_train['N'].asfreq(freq='D'))
forecaster

steps = 20
predictions = forecaster.predict(steps=steps)
predictions.head(20)

print ('Hello')

fig, ax = plt.subplots(figsize=(9, 4))
data_train['N'].plot(ax=ax, label='train')
data_test['N'].plot(ax=ax, label='test')
predictions.plot(ax=ax, label='predictions')
ax.legend();


print ('Hello2')
predictions.head(20)
