using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApiSwc.DTO.XML
{

    [Serializable]
    [XmlRoot("tlist")]
    public class CreepingLine4XmlDto
    {
        public int Id { get; set; }

        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("start_time")]
        public long StarTime { get; set; }

        [XmlElement("duration")]
        public int Duration { get; set; }
    }

    //<tlist>
    //<key>c749ea14-9d9a-405c-aa2e-6f46c25f7d8c</key>
    //<message>уважаемые встречающие скоростной электропоезд Стрела сообщением Н.Новгород С.Петербург прибывает на 5 путь платформа номер 4 скоростной электропоезд Стрела сообщением Н.Новгород С.Петербург прибывает на 5 путь платформа номер 4 будьте внимательны и осторожны </message>
    //<start_time>1603780890073</start_time>
    //<duration>19052</duration>
    //<scoreboards>reg</scoreboards>
    //</tlist>

}