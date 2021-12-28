using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BU
{
    public enum PokerType : int { 方塊 = 0x00, 梅花 = 0x10, 紅心 = 0x20, 黑桃 = 0x30, }

    public enum PokerName : int
    {
        方塊_A = PokerType.方塊 + 1, 方塊_2, 方塊_3, 方塊_4, 方塊_5, 方塊_6, 方塊_7, 方塊_8, 方塊_9, 方塊10, 方塊_J, 方塊_Q, 方塊_K,
        梅花_A = PokerType.梅花 + 1, 梅花_2, 梅花_3, 梅花_4, 梅花_5, 梅花_6, 梅花_7, 梅花_8, 梅花_9, 梅花10, 梅花_J, 梅花_Q, 梅花_K,
        紅心_A = PokerType.紅心 + 1, 紅心_2, 紅心_3, 紅心_4, 紅心_5, 紅心_6, 紅心_7, 紅心_8, 紅心_9, 紅心10, 紅心_J, 紅心_Q, 紅心_K,
        黑桃_A = PokerType.黑桃 + 1, 黑桃_2, 黑桃_3, 黑桃_4, 黑桃_5, 黑桃_6, 黑桃_7, 黑桃_8, 黑桃_9, 黑桃10, 黑桃_J, 黑桃_Q, 黑桃_K,
        鬼牌大 = 0x3E, 鬼牌小 = 0x3F
    }
}