using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class patkany : MonoBehaviour {

    int workDayStartHour = 9;
    int workDayEndHour = 17;
    int availableWorkingMinutesOnCurrentWorkingDay = 0;

    DateTime TaskAssigner(DateTime DateTaskReceived, int HoursRequiredForTask)
    {
        int minutesRequiredToFinishTask = HoursRequiredForTask * 60;
        DateTime sortingDate = DateTaskReceived;

        while (0 < minutesRequiredToFinishTask)
        {

            if (IsWorkdDay(sortingDate) == false)
            {
                sortingDate = sortingDate.AddDays(1);
                sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStartHour, 0, 0);
            }

            if ( IsWorkdDay(sortingDate) == true)
            {
                switch (WhatTimeOfWorkingHours(sortingDate))
                {
                    case "beforeWorkingHours":
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStartHour, 0, 0);
                        break;

                    case "duringWorkingHours":
                        availableWorkingMinutesOnCurrentWorkingDay = ((workDayEndHour - sortingDate.Hour) * 60) + (60 - sortingDate.Minute) % 60;

                        if (TaskCanBeFinishedToday(availableWorkingMinutesOnCurrentWorkingDay, minutesRequiredToFinishTask) == true)
                        {
                            sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, (sortingDate.Hour + minutesRequiredToFinishTask / 60), (sortingDate.Minute + minutesRequiredToFinishTask % 60), 0);
                            minutesRequiredToFinishTask = 0;
                        }
                        else if (TaskCanBeFinishedToday(availableWorkingMinutesOnCurrentWorkingDay, minutesRequiredToFinishTask) == false)
                        {
                            minutesRequiredToFinishTask = minutesRequiredToFinishTask - availableWorkingMinutesOnCurrentWorkingDay;
                            sortingDate = sortingDate.AddDays(1);
                            sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStartHour, 0, 0);
                        }
                        break;

                    case "afterWorkingHours":
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day + 1, workDayStartHour, 0, 0);
                        break;
                }
            }

        }
        return sortingDate;
    }

    bool IsWorkdDay (DateTime dateToCheck)
    {
        bool isWorkDay = true;
        if (dateToCheck.DayOfWeek == DayOfWeek.Saturday || dateToCheck.DayOfWeek == DayOfWeek.Sunday)
        {
            isWorkDay = false;
            return isWorkDay;
        }
        else
        {
            isWorkDay = true;
            return isWorkDay;
        }
    }

    string WhatTimeOfWorkingHours (DateTime dateToCheck)
    {
        string period = null;

        if (dateToCheck.Hour < workDayStartHour)
        {
            period = "beforeWorkingHours";
        }

        if (dateToCheck.Hour >= workDayStartHour && dateToCheck.Hour < workDayEndHour)
        {
            period = "duringWorkingHours";
        }

        if (dateToCheck.Hour > workDayEndHour)
        {
            period = "afterWorkingHours";
        }

        return period;
    }

    bool TaskCanBeFinishedToday (int availableMinutes, int requiredMinutes)
    {
        bool canBeFinished = false;

        if (availableMinutes - requiredMinutes >= 0)
        {
            canBeFinished = true;
        }

        if (availableMinutes - requiredMinutes < 0)
        {
            canBeFinished = false;
        }

        return canBeFinished;
    }





	// Use this for initialization
	void Start () {

        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 0, 0), 5) == new DateTime(2018, 6, 4, 15, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 0, 0), 10) == new DateTime(2018, 6, 5, 12, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday, can NOT be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday, can NOT be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 7, 0, 0), 4) == new DateTime(2018, 6, 4, 13, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday BEFORE working hours, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday BEFORE working hours, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 18, 0, 0), 7) == new DateTime(2018, 6, 5, 16, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday AFTER working hours, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday AFTER working hours, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 16, 11, 0, 0), 2) == new DateTime(2018, 6, 18, 11, 0, 0))
        {
            Debug.Log("CORRECT: Task received during weekend, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during weekend, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 12, 31, 10, 0, 0), 18) == new DateTime(2019, 1, 2, 12, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workdays, can only be completed next year, two days from receive date");
        }
        else
        {
            Debug.Log("WRONG: Task received during workdays, can only be completed next year, two days from receive date");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 36, 0), 5) == new DateTime(2018, 6, 4, 15, 36, 0))
        {
            Debug.Log("CORRECT: Task received during workdays, can only be completed next year, two days from receive date");
        }
        else
        {
            Debug.Log("WRONG: Task received during workdays, can only be completed next year, two days from receive date");
        }

    }

}
