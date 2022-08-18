using System.ComponentModel.DataAnnotations;

namespace WebApiSwc.DTO.JSON.OptionsDto.PagingOption
{
    public class PagedOptionDto
    {
        [Range(1,100)]
        public int Count { get; set; }
        
        [Range(1000, 120000)]   //от 1сек до 2мин
        public int Time { get; set; }    
    }
}