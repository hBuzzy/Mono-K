using System;
using System.Threading.Tasks;

public abstract class AsyncExtensions
{
   public static async Task WaitForSeconds(float seconds)
   {
      await Task.Delay(TimeSpan.FromSeconds(seconds));
   }   
}