using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lucene.Mvc.Models
{
    public static class SampleDataRepository
    {

        public static List<SampleData> _theList = new List<SampleData>();
        public static SampleData Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
        }

        public static List<SampleData> GetAll()
        {
            LoadOldData();
            LoadKidioData();
            return _theList;
        }

        private static void Insert(int id, string name, string description, string category)
        {
            _theList.Add(new SampleData { Id = id, Name = name, Description = description, Category = category });
        }

        private static void LoadOldData()
        {
            Insert(1, "Belgrade", "City in Serbia", "Cities");
            Insert(2, "Moscow", "City in Russia", "Cities");
            Insert(3, "Chicago", "City in USA", "Cities");
            Insert(4, "Mumbai", "City in India", "Cities");
            Insert(5, "Hong Kong", "City in Hong Kong", "Cities");
        }

        private static void LoadKidioData()
        {
            string data = @"6,Outdoor playground in Greenstone that is close to everything as weel as Stone Ridege Mall and a park,Outdoor Joy,Playground;
            7,Indoor trampoline playground that is easily accessible - locations in Bedfordview, Edenvale and Greenstone,Bounce,Playground;
            8,Cape Toddler is located in the Edevale and Dunvegan Shopping Centres - close to home and schools with 35 choice shops,Dunvegan Centre,Shopping;
            9,Pet Shop in Edenvale,Happy Pet,Shopping;
            10,Children's School Requirements in Kempton park,The Red Pencil,Shopping;
            11,Latest and greatest toys, Lego, books and other educational toys,Barbarella's,Shopping;
            12,Kwarri will transport your little precious bundle of joy anywhere you desire.,Kwarri Cars,Transport Services;
            13,Swing and climb enormous pine tree in your back yard,Go Ape,Outdoor Events;
            14,Crash your bike and break your bones while racing on our muddy but fun course,Bike Smash,Outdoor Events;
            15,Run and and walk in the park at Giloollies Farm on the outskirts of Edenvale and Bedfordview.,The Farm,Outdoor Events;
            15,Skating, roller blading and skateboarding all day long until you cry.,Skate Park,Outdoor Events;
            16,Modderfontein school for gifted children,Montessori,Schools;
            17,Pre-primary school in Hurleyvale, Hurleyvale Primary and Pre-School,Schools;
            18,High School located in a secure and safe village environment,Essori High School Modderfontein,Schools;
            19,Veld School wehere you can learn all about the bush at your own pace while tracking wild animals,Bush and Beach Adventures,Schools;
            20,School of Life and Hard Knocks - come and see a real fireman!,Fireman's Area,Schools;";


            string[] list = data.Trim().Split(';');
            string[] theLine = null;
            for (int i = 0; i < list.Length; i++)
            {
                for (int k = 0; k < list.Length; k++)
                {

                    if (!string.IsNullOrWhiteSpace(list[k]))
                    {
                        theLine = list[k].Split(',');
                    }
                    Insert(int.Parse(theLine[0]), theLine[1], theLine[2], theLine[3]);
                }
            }
        }
    }
}