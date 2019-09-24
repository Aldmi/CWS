namespace Shared.MoqObj
{
    /// <summary>
    /// Moq sibWay display driver
    /// </summary>
    public class DisplayDriver
    {
        public void Initialize(string address, ushort port)
        {

        }

        public int OpenConection()
        {
            return 0;//  ERROR_SUCCESS = 0,
        }


        public int SendMessage(byte screen, DisplayEffect effect, DisplayTextHAlign textHAlign, DisplayTextVAlign textVAlign, ushort displayTime, DisplayTextHeight textHeight, uint color, string text)
        {
            return 0; //  ERROR_SUCCESS = 0,
        }


        public bool SetTime(System.DateTime dateTime)
        {
            return true;
        }



        public void Dispose()
        {

        }
    }



    public enum DisplayEffect
    {
        SimpleDisplay
    }


    public enum DisplayTextColor
    {
        White
    }


    public enum DisplayTextHAlign
    {
        Center,
        Right
    }

    public enum DisplayTextHeight
    {
        px12,
        px16
    }

    public enum DisplayTextVAlign
    {
        Bottom,
        Center,
        Top
    }

}