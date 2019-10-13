using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {
    public float DayCounter; //Counts the amount of days that have passed

    public float DayTimeLength; //Sets how long each day lasts for. Higher values make the day go faster
    public float NightTimeLength; //Sets how long each night lasts for. Higher values make the night go faster

    public bool isDayTime; //On off Switch for Day time. Can use this so that things have different attributes during the day
    public bool isNightTime; //On off Switch for Night time. Can use this so that things have different attributes during the Night

    private bool EndOfDay; //Variable used to isolate the point in time when the next day will start 
    bool nextDay; //A check to see if it's time to move onto the next day.


    // Update is called once per frame
    void Update() {
        //Grabs the rotation of the Directional Light. These rotations are the period of time that there is light, creating the Day cycle
        if (transform.eulerAngles.x > 0 && transform.eulerAngles.x < 180) {
            isNightTime = false; //Turns the night time switch off because it's day time
            isDayTime = true; // Turns the day time switch on because it's day time
        }

        //Grabs the rotation of the Directional Light. These rotations are the period of time that there is darkness, creating the Night cycle
        else if (transform.eulerAngles.x > 180 && transform.eulerAngles.x < 360) {
            isDayTime = false; //Turns the day time switch off because it's night time
            isNightTime = true; //Turns the night time switch on because it's night time

        }

        //While the Directional Light is in the Day Cycle it'll rotate the camera on it's x axis to progress time at the speed of the Day Cycle.
        if (isDayTime)
            transform.Rotate(1 * DayTimeLength * Time.deltaTime, 1 * DayTimeLength * Time.deltaTime, 0);

        //While the Directional Light is in the Day Cycle it'll rotate the camera on it's x axis to progress time at the speed of the Night Cycle.
        if (isNightTime)
            transform.Rotate(1 * NightTimeLength * Time.deltaTime, 1 * DayTimeLength * Time.deltaTime, 0);

        //Function for increasing the DayCounter Variable
        IncreaseNumberOfDaysSurvived();
    }


    void IncreaseNumberOfDaysSurvived() {

        //Finds rough rotation in which we want to end a full day (midnight).
        if (transform.eulerAngles.x > 270 && transform.eulerAngles.x < 280) {
            EndOfDay = true; //Sets the EndOfDay variable so that we can get ready to progress onto the next day
        } else {
            nextDay = false; //if it's no longer the end of day then we set the nextDay variable to false as the next day is the current day.
            EndOfDay = false; //We reset the end of day. NOTE we reset this after we set nextDay to false so that there is a frame for the DayCounter to increment.
        }

        //If it's the end of the day but not the next day yet then we make the transition to the next day and increase the DayCounter to the next day.
        if (EndOfDay && !nextDay) {
            nextDay = true; //This variable only holds true for a frame and then resets back to false so that the counter can increment only once.
            DayCounter += 1; //Increases the days passed counter.
        }

    }

}