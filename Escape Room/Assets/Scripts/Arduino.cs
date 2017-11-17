using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{

    //Serial port values
    public string PortName = "";
    public int BaudRate = 9600;
    public char StartFlag = '#';
    public int PollingRate = 100;
    public int PackagesLost = 0;
    public int readTimeouts = 0;
    public bool ParseIncomingData = true;
    private SerialPort arduino;
    private int retries = 0;
    private IEnumerator SerialUpdate;

    //Incoming and outgoing data
    public string NewestIncomingData = "";
    public string NewestOutgoingData = "";
    public uint ArduinoMillis = 0;
    public bool ButtonOne;
    public bool ButtonTwo;

    //Event handler
    public delegate void NewDataEventHandler(Arduino arduino);
    public static event NewDataEventHandler NewDataEvent;

    // Use this for initialization
    void Start()
    {
        OpenPort(); //Open the serial port when the scene is loaded.
    }

    // My Arduino script uses coroutines instead of Update, to enable faster serial communication than the frame rate
    void Update()
    {


    }

    //Process the data we get from our Arduino (this function might be called more often than Update(), depending on the chosen polling rate)
    private void ProcessInputFromArduino(string serialInput)
    {
        NewestIncomingData = serialInput;

        if (!ParseIncomingData)
            return;

        // ----- INPUT FROM ARDUINO TO UNITY ----- //
        //From here you can do what ever you want with the data.
        //As an example, I parse the data into public variables that can be accessed from other classes/scripts:
        string[] values = serialInput.Split('\t');  //Split the string between the chosen delimiter (tab)

        ArduinoMillis = uint.Parse(values[0]);      //Pass the first value to an unsigned integer
        ButtonOne = StringToBoolean(values[1]); //Get value from button on pin 2
        ButtonTwo = StringToBoolean(values[2]); //Get value from button on pin 3

        //Feel free to add new variables (both here and in the Arduino script).

        //When ever new data arrives, the scripts fires an event to any scripts that are subscribed, to let them know there is new data available (e.g. my Arduino Logger script).
        if (NewDataEvent != null)   //Check that someone is actually subscribed to the event
            NewDataEvent(this);     //Fire the event in case someone is

        //To subscribe to the event you can write:
        //  Arduino.NewDataEvent += NewData;

        //where NewData is the name of a function that should be called when an event fires, e.g.:
        //  void NewData(Arduino arduino)
        //  {
        //    doSomething();
        //  }
    }

    bool StringToBoolean(string str) {
        if (int.Parse(str) == 1)
            return true;
        else 
            return false;
    }

    // ----- SERIAL COMMUNICATION ----- //
    //The code below is handling everything else concerning the serial communication.
    //You shouldn't need to change any of it. If you have any improvements or find any bugs, please email me.


    //Buffers used for serial input
    private byte[] readBuffer = new byte[4096];
    private string inputBuffer = "";
    private IEnumerator ReadIncomingData()
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        while (true) //Loop until stopped by StopCoroutine()
        {
            try
            {
                //Read everything currently in the system input buffer
                int bytesRead = arduino.Read(readBuffer, 0, readBuffer.Length);
                //Convert the byte to ASCII (a string)
                string serialInput = encoder.GetString(readBuffer, 0, bytesRead);
                //Add the new data to our own input buffer
                inputBuffer += serialInput;

                //Find a new line flag (indicates end of a data package)
                int endFlagPosition = inputBuffer.IndexOf('\n');
                //If we found a flag, process it further
                while (endFlagPosition > -1)
                {
                    //Check if the start flag is also there (i.e. we have recieved an entire data package
                    if (inputBuffer[0] == StartFlag)
                    {
                        //Hand the data to the function above
                        ProcessInputFromArduino(inputBuffer.Substring(1, endFlagPosition));
                    }
                    else
                    //If the start flag isn't there, we have only recieved a partial data package, and thus we throw it out
                    {
                        if (PackagesLost > 0) //Don't complain about first lost package, as it usually happens once at startup
                            Debug.Log("Start flag not found in serial input (corrupted data?)");
                        PackagesLost++; //Count how many packages we have lost since the start of the scene.
                    }

                    //Remove the data package from our own input buffer (both if it is partial and if it is complete)
                    inputBuffer = inputBuffer.Remove(0, endFlagPosition + 1);
                    //Check if there is another data package available in our input buffer (while-loop). Makes sure we're not behind and only read old data (e.g. if Unity hangs for a second, the Arduino would have send a lot of packages meanwhile that we need to handle)
                    endFlagPosition = inputBuffer.IndexOf('\n');
                }
                //Reset the timeout counter (as we just recieved some data)
                readTimeouts = 0;
            }
            catch (System.Exception e)
            {
                //Catch any timeout errors (can happen if the Arduino is busy with something else)
                readTimeouts++;

                //If we time out many times, the something is propably wrong with serial port, in which case we will try to reopen it.
                if (readTimeouts > 200)
                {
                    Debug.Log("No data recieved for a long time (" + PortName + ").\n" + e.ToString());
                    ReopenPort();
                }
            }
            //Make the coroutine take a break, to allow Unity to also use the CPU.
            //This currently doesn't account for the time the coroutine actually takes to run (~1ms) and thus isn't the true polling rate.
            yield return new WaitForSeconds(1.0f / PollingRate);
        }
    }

    void ReopenPort()
    {
        Debug.Log("Trying to reopen SerialPort with name " + PortName + ". Try #" + retries);
        StopCoroutine(SerialUpdate);
        arduino.Close();
        readTimeouts = 0;
        PackagesLost = 0;
        retries++;
        if (retries > 5)
        {
            Debug.LogError("Couldn't open serial port with name " + PortName);
            gameObject.SetActive(false);
            return;
        }
        Invoke("OpenPort", 5f);

    }

    void OpenPort()
    {
        arduino = new SerialPort(PortName, BaudRate);
        arduino.ReadTimeout = 1000;
        arduino.WriteTimeout = 50; //Unfortunatly 

        try
        {
            arduino.Open();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Serial Port with name " + PortName + " could not be opened. Try one of these instead:");
            foreach (var portName in SerialPort.GetPortNames())
            {
                Debug.Log(portName);
            }

            return;
        }


        if (!arduino.IsOpen)
        {
            Debug.LogError("Couldn't open Serial Port with name " + PortName);
            gameObject.SetActive(false);
            return;
        }

        //Clear any data in the buffer (the C# methods made for this in the Serial class are not implemented in this version of Mono)
        try
        {
            byte[] buffer = new byte[arduino.ReadBufferSize];
            arduino.Read(buffer, 0, buffer.Length);
        }
        catch (System.Exception)
        {
            // ignored
        }


        arduino.ReadTimeout = 1; //We don't want Unity to hang in case there's no data yet. Better to timeout the reading and let Unity do other things while waiting for new data to arrive

        SerialUpdate = ReadIncomingData();
        StartCoroutine(SerialUpdate);
    }

    void OnDisable()
    {
        StopCoroutine(ReadIncomingData());
        arduino.Close();
    }
}
