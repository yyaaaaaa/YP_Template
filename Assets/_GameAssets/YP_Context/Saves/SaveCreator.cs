using System;
using static YP.Saves;


namespace YP
{
    public static class SaveCreator
    {
        
        public static void Create(StartSaveValues startValues)
        {
            new ItemInt(Key_Save.test_count, 0);
            new ItemBool(Key_Save.ads_enabled, true);
            new ItemString(Key_Save.last_time, DateTime.Now.ToString());

        }


    }
}


