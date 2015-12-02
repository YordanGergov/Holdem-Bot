﻿namespace TexasHoldem.AI.SmartPlayer.Helpers
{
    public enum CardValuationType
    {
        Unplayable = 0,
        NotRecommended = 1000,
        Risky = 2000,
        Recommended = 3000,
        Premium = 4000,
        TopPremium = 5000
    }
}
