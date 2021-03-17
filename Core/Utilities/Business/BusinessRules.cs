using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        /// <summary>
        /// İş kuralı barındıran private metotları eklenme sırasına göre kontrol eder.
        /// </summary>
        /// <param name="appliedLogics"></param>
        /// <returns></returns>
        public static IResult CheckAll(params IResult[] appliedLogics)
        {
            foreach (var result in appliedLogics)
            {
                if (!result.Success)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
