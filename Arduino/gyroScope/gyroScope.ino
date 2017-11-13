// Output Frequency = 1000 
// serialOutputInterval = 1000 
// 10 = 100Hz
const unsigned int serialOutputInterval = 10; 
unsigned long serialLastOutput = 0;

const char StartFlag = '#';
const String Delimiter = "\t";

bool buttonOne = false;
bool buttonTwo = false;

int BTN_ONE_PIN = 2;
int BTN_TWO_PIN= 3;


// ================================================================
// ===                      INITIAL SETUP                       ===
// ================================================================

void setup() {

    // initialize serial communication
    Serial.begin(9600);

    //Set input 2 and 3 to input
    pinMode(BTN_ONE_PIN, INPUT);
    pinMode(BTN_TWO_PIN, INPUT);
}


// ================================================================
// ===                    MAIN PROGRAM LOOP                     ===
// ================================================================

void loop() {
        //Check if it is time to send data to Unity
        SerialOutput();
}

void SerialOutput() {
  //Time to output new data?
  if(millis() - serialLastOutput < serialOutputInterval)
    return;
  serialLastOutput = millis();

  //Write data package to Unity
  Serial.write(StartFlag);    //Flag to indicate start of data package
  Serial.print(millis());     //Write the current "time"
  Serial.print(Delimiter);    //Delimiter used to split values
  Serial.print(digitalRead(BTN_ONE_PIN));       //Write a value
  Serial.print(Delimiter);    //Write delimiter
  Serial.print(digitalRead(BTN_TWO_PIN));       //...
  Serial.println();           // Write endflag '\n' to indicate end of package

}
