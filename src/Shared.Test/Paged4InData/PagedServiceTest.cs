using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Shared.Paged;
using Xunit;

namespace Shared.Test.Paged4InData
{
    public class PagedServiceTest
    {
        [Fact]
        public async Task CalculateNextPageTest()
        {
            var pagedService = new PagedService<int>(new PagedOption {Count = 3, Time = 200});
            pagedService.NextPageRx.Subscribe(res =>
            {
                Debug.WriteLine(res.ToArray().Select(v => v.ToString()).Aggregate((s1, s2) => $"{s1} {s2}"));
            });


            var bufer = Enumerable.Range(1, 10).ToArray();
            pagedService.SetData(bufer);
            await Task.Delay(2000);
            Debug.WriteLine("----------------------------");
            
            bufer = Enumerable.Range(10, 5).ToArray();
            pagedService.SetData(bufer);
            await Task.Delay(2000);
            Debug.WriteLine("----------------------------");
            
            await Task.Delay(-1);
        }


        [Fact]
        public async Task CalculateNextPage_Empty_SetData_Test()
        {
            var pagedService = new PagedService<int>(new PagedOption {Count = 3, Time = 2000});
            pagedService.NextPageRx.Subscribe(res =>
            {
                if (!res.Any())
                    Debug.WriteLine("IsEmpty");
                else
                {
                    Debug.WriteLine(res.ToArray().Select(v => v.ToString()).Aggregate((s1, s2) => $"{s1} {s2}"));
                }
            });


            var bufer = Enumerable.Range(1, 10).ToArray();
            pagedService.SetData(bufer);
            await Task.Delay(1000);
            //Debug.WriteLine("----------------------------");

            bufer = Enumerable.Range(1, 0).ToArray();
            pagedService.SetData(bufer);
            //await Task.Delay(2000);
            // Debug.WriteLine("----------------------------");


            await Task.Delay(-1);
        }


        [Fact]
        public async Task CalculateNextPage_Null_SetData_Test()
        {
            var pagedService = new PagedService<int>(new PagedOption {Count = 3, Time = 200});
            pagedService.NextPageRx.Subscribe(res =>
            {
                if (!res.Any())
                    Debug.WriteLine("IsEmpty");
                else
                {
                    Debug.WriteLine(res.ToArray().Select(v => v.ToString()).Aggregate((s1, s2) => $"{s1} {s2}"));
                }
            });


            var bufer = Enumerable.Range(1, 10).ToArray();
            pagedService.SetData(bufer);
            await Task.Delay(1000);
            Debug.WriteLine("----------------------------");

            bufer = null;
            pagedService.SetData(bufer);
            await Task.Delay(5000);
            Debug.WriteLine("----------------------------");


            bufer = Enumerable.Range(100, 5).ToArray();
            pagedService.SetData(bufer);
            await Task.Delay(1000);
            Debug.WriteLine("-----------------------------");


            await Task.Delay(-1);
        }
    }
}