using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace CareMobileApp.Utils
{
    public class SamplePersonType
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Profession { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public static class DataService
    {
        private static List<SamplePersonType> _persons;

        static DataService()
        {
            _persons = new List<SamplePersonType>();

            _persons.Add(new SamplePersonType() { FirstName = "Glenn", LastName = "Versweyveld", Profession = "Mobile developer" });
            _persons.Add(new SamplePersonType() { FirstName = "Bart", LastName = "Lannoeye", Profession = ".Net architect" });
            _persons.Add(new SamplePersonType() { FirstName = "Jan", LastName = "Van de Poel", Profession = "Entrepreneur" });
        }

        public static List<SamplePersonType> GetPersons()
        {
            return _persons;
        }
    }

    public static class NavigationMessage
    {
        public static void PutData<T>(string key, T value)
        {
            Application.Current.Properties[key] = value;
        }

        public static T GetData<T>(string key)
        {
            T value = default(T);

            IDictionary<string, object> iDictionary = Application.Current.Properties;

            if (iDictionary.ContainsKey(key))
            {
                value = (T)iDictionary[key];
            }

            return value;
        }
    }

    public static class Const
    {
        public const string JobApplicationPagesData = "JobApplicationPagesData";
    }

    public class JobApplicationPagesData
    {
        public MemoryStream PhotoStream { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string SelectedPosition { get; set; }

        public bool IsValid
        {
            get
            {
                var returnValue = false;

                if (
                    PhotoStream != null
                    ||
                    !String.IsNullOrEmpty(FullName)
                    ||
                    !String.IsNullOrEmpty(EmailAddress)
                    ||
                    BirthDate != DateTime.Now
                    ||
                    !String.IsNullOrEmpty(SelectedPosition)
                    )
                {
                    returnValue = true;
                }

                return returnValue;
            }
        }
    }

    public static class JobApplicationPagesDataManager
    {
        public static JobApplicationPagesData Instance
        {
            get
            {
                var instance = NavigationMessage.GetData<JobApplicationPagesData>(Const.JobApplicationPagesData);
                if (instance == null)
                {
                    instance = new JobApplicationPagesData();
                    NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, instance);
                }
                return instance;
            }
        }
        public static void Clear()
        {
            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, null);
        }
        public static void SetPhotoStream(MemoryStream ms)
        {
            Instance.PhotoStream = ms;
            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, Instance);
        }
        public static void SetFormData(JobApplicationPagesData formData)
        {
            Instance.BirthDate = formData.BirthDate;
            Instance.EmailAddress = formData.EmailAddress;
            Instance.FullName = formData.FullName;
            Instance.SelectedPosition = formData.SelectedPosition;

            NavigationMessage.PutData<JobApplicationPagesData>(Const.JobApplicationPagesData, Instance);
        }
    }
}
