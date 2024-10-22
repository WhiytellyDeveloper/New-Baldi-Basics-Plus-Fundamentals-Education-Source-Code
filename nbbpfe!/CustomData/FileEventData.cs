namespace nbppfe.CustomData
{
    public class FileEventData
    {
        public string name = "None";
        public string voicelineName = "NoneEvent";
        public string voicelineEventFirstKey = "Vfx_BAL_NoneEvent0";

        public SubtitleTimedKey[] voicelinesKeys = [
            new SubtitleTimedKey {
                key = "Vfx_BAL_NoneEvent1",
                time = 0
             },
            new SubtitleTimedKey {
                key = "Vfx_BAL_NoneEvent2",
                time = 0
             }
        ];

        public int minTime = 60;
        public int maxTime = 120;
    }
}
