#include <SoftwareSerial.h>
#include <Wire.h>


#define RX 10
#define TX 11
String AP = "DIGI-37Bt";       // CHANGE ME
String PASS = "sE8SC2JE"; // CHANGE ME
String API = "2063CFKFUTBRH1T9";   // CHANGE ME
String HOST = "api.thingspeak.com";
String PORT = "80";
String field = "val1";
int countTrueCommand;
int countTimeCommand; 
boolean found = false; 
int valSensor1 = 1;
int valSensor2 = 2;
int valSensor3 = 3;

const byte nitro[] = {0x01,0x03, 0x00, 0x1e, 0x00, 0x01, 0xe4, 0x0c};
const byte phos[] = {0x01,0x03, 0x00, 0x1f, 0x00, 0x01, 0xb5, 0xcc};
const byte pota[] = {0x01,0x03, 0x00, 0x20, 0x00, 0x01, 0x85, 0xc0};

#define RE 8
#define DE 7
byte values[11];
SoftwareSerial mod(2,3);

SoftwareSerial wifiSensor(RX,TX); 
 
  
void setup() {
  Serial.begin(9600);
  mod.begin(9600);
  pinMode(RE, OUTPUT);
  pinMode(DE, OUTPUT);  
  wifiSensor.begin(115200);
  sendCommand("AT",5,"OK");
  sendCommand("AT+CWMODE=1",5,"OK");
  sendCommand("AT+CWJAP=\""+ AP +"\",\""+ PASS +"\"",20,"OK");
    delay(2000);

}
void loop() {
    int val1,val2,val3;
    val1 = nitrogen();
    delay(250);
    val2 = phosphorous();
    delay(250);
    val3 = potassium();
 String getData = "GET /update?api_key="+ API +"&"+ "field1" +"="+String(val1)+ "&field2" +"="+String(val2)+ "&field3" +"="+String(val3);
 sendCommand("AT+CIPMUX=1",5,"OK");
 sendCommand("AT+CIPSTART=0,\"TCP\",\""+ HOST +"\","+ PORT,15,"OK");
 sendCommand("AT+CIPSEND=0," +String(getData.length()+4),4,">");
 wifiSensor.println(getData);delay(1500);countTrueCommand++;
 sendCommand("AT+CIPCLOSE=0",5,"OK");
}
int getSensorData(){
  return random(1000); // Replace with 
}
void sendCommand(String command, int maxTime, char readReplay[]) {
  while(countTimeCommand < (maxTime*1))
  {
    wifiSensor.println(command);//at+cipsend
    if(wifiSensor.find(readReplay))//ok
    {
      found = true;
      break;
    }
  
    countTimeCommand++;
  }
  
  if(found == true)
  {
    countTrueCommand++;
    countTimeCommand = 0;
  }
  
  if(found == false)
  {
    countTrueCommand = 0;
    countTimeCommand = 0;
  }
  
  found = false;
 }


int nitrogen(){
  byte val[7];
  byte flag = 1;
  int rezultat = 0;
  
  digitalWrite(DE,HIGH);
  digitalWrite(RE,HIGH);
  delay(10);
  Serial.write(nitro,sizeof(nitro));
  delay(10);
  digitalWrite(DE,LOW);
  digitalWrite(RE,LOW);
  delay(50);
  Serial.println("val citire n");

  while(Serial.available()){
    val[0] = Serial.read();
    if(val[0]){
      Serial.print(val[0],HEX);
      Serial.print(' ');
      for(byte i=1;i<7;i++){
        val[i] = Serial.read();
        Serial.print(val[i],HEX);
        Serial.print(' ');
      }
    }
  }
  Serial.println();  
  
  rezultat = (val[3] << 8) | val[4];
  
  return rezultat;
}

int phosphorous(){
  byte val[7];
  byte flag = 1;
  int rezultat = 0;
  
  digitalWrite(DE,HIGH);
  digitalWrite(RE,HIGH);
  delay(10);
  Serial.write(phos,sizeof(phos));
  delay(10);
  digitalWrite(DE,LOW);
  digitalWrite(RE,LOW);
  delay(50);
  Serial.println("val citire p");
  while(Serial.available()){
    val[0] = Serial.read();
    if(val[0]){
      Serial.print(val[0],HEX);
      Serial.print(' ');
      for(byte i=1;i<7;i++){
        val[i] = Serial.read();
        Serial.print(val[i],HEX);
        Serial.print(' ');
      }
    }
  }
  Serial.println();
  
  rezultat = (val[3] << 8) | val[4];
  
  return rezultat;
}

int potassium(){
  byte val[7];
  byte flag = 1;
  int rezultat = 0;
  
  digitalWrite(DE,HIGH);
  digitalWrite(RE,HIGH);
  delay(10);
  Serial.write(pota,sizeof(pota));
  delay(10);
  digitalWrite(DE,LOW);
  digitalWrite(RE,LOW);
  delay(50);
  Serial.println("val citire k");
  while(Serial.available()){
    val[0] = Serial.read();
    if(val[0]){
      Serial.print(val[0],HEX);
      Serial.print(' ');
      for(byte i=1;i<7;i++){
        val[i] = Serial.read();
        Serial.print(val[i],HEX);
        Serial.print(' ');
      }
    }
  }
  Serial.println();
  
  rezultat = (val[3] << 8) | val[4];
  
  return rezultat;
}