using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Onslaught
{
    class FormulaEvaluator
    {

        Dictionary<String, int> statHolder;

        public FormulaEvaluator()
        {
           
        }

        public double Evaluate(String form, Unit supp)
        {
            Char dummy = new Char();
            return Evaluate(form, supp, dummy);
        }

        //evaluates a damageformula string of the form "str + 2 * mag / eDef"
        public double Evaluate(String form, Unit att, Unit def)
        {
            statHolder = new Dictionary<String, int>();
            statHolder.Add("str", (int)(att.str * att.buffList[0]));
            statHolder.Add("mag", (int) (att.mag * att.buffList[1]));
            statHolder.Add("def", (int) (att.def * att.buffList[2]));
            statHolder.Add("fth", (int)(att.fth * att.buffList[3]));
            statHolder.Add("ski", (int)(att.ski * att.buffList[4]));
            statHolder.Add("eva", (int)(att.eva * att.buffList[5]));
            statHolder.Add("luck", (int)(att.luck * att.buffList[6]));
            statHolder.Add("spd", (int)(att.spd * att.buffList[7]));

            statHolder.Add("eStr", (int)(def.str * def.buffList[0]));
            statHolder.Add("eMag", (int)(def.mag * def.buffList[1]));
            statHolder.Add("eDef", (int)(def.def * def.buffList[2]));
            statHolder.Add("eFth", (int)(def.fth * def.buffList[3]));
            statHolder.Add("eSki", (int)(def.ski * def.buffList[4]));
            statHolder.Add("eEva", (int)(def.eva * def.buffList[5]));
            statHolder.Add("eLuck", (int)(def.luck * def.buffList[6]));
            statHolder.Add("eSpd", (int)(def.spd * def.buffList[7]));

            double total;
            try
            {
                total = (double)statHolder[form.Substring(0, form.IndexOf(" "))];
            }
            catch
            {
                total = Convert.ToDouble(form.Substring(0, form.IndexOf(" ")));
            }
            double placeHolder;
            form = form.Substring(form.IndexOf(" ")+1) + " ";
            
            while (!(form.Equals("")))
            {


                try
                {
                    placeHolder = (double) statHolder[form.Substring(2, form.Substring(2).IndexOf(" "))];
                }
                catch
                {

                    
                    placeHolder =  Convert.ToDouble(form.Substring(2, form.Substring(2).IndexOf(" ")));

                }
                if (form.Substring(0, 1).Equals("+")) total += placeHolder ;
                if (form.Substring(0, 1).Equals("-")) total -= placeHolder;
                if (form.Substring(0, 1).Equals("*")) total *= placeHolder;
                if (form.Substring(0, 1).Equals("/")) total /= placeHolder;
                
                form = form.Substring(form.Substring(2).IndexOf(" ") + 3);

            }
            
            return total;
        }

        
    }
}
