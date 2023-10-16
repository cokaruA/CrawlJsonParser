using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerJsonParser
{
    public enum SpinDataKey
    {// Do Not Change Order  - Chilli Heat
        _id,
        index,
                
        //reel_set,

        balance,
        balance_bonus,
        balance_cash,

        bl,
        c,
        counter,

        g,

        I,
        l0,
        l1,
        l2,
        l3,
        l4,
        l5,

        linepay,
        na,

        rs,
        rs_c,
        rs_m,
        rs_p,

        s,
        sh,
        st,
        sver,

        mo,
        mo_t,
        tmb_res,
        tmb_win,
        tw,
        w,                          
    };


    public enum ReelDataSet
    {
        reg,
        top,
    };

    public enum ReelDataKey
    {
        reel_set,
        s,
        sa,
        sb,
        sh,
        st,
        sw
    };
}
