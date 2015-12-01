﻿namespace TexasHoldem.AI.SmartPlayer
{
    using System;

    using TexasHoldem.AI.SmartPlayer.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;


    public class GoshoTheBot : BasePlayer
    {
        // calculation implented at start of preflop round
        // not sure if MoneyLeft property is OUR money left?!?!
        public static int mFactor;
        //implemented
        public static bool hasTheButton = false;

        //we should find who has iniative
        public static bool iniative;

        ////we should find if we are in a 3bettedPot
        public static bool is3bettedPot = false;

        public override string Name { get; } = "Gosho" + Guid.NewGuid();
        
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            #region PreFlopLogic            
            if (context.RoundType == GameRoundType.PreFlop)
            {
                //checks if we have the button
                if (context.MyMoneyInTheRound == context.SmallBlind)
                {
                    hasTheButton = true;
                }

                //calcutes M factor
                // not sure if MoneyLeft property is OUR money left?!?!
                mFactor = context.MoneyLeft / (context.SmallBlind * 2) ;
               

                var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);

                //all in baby; just to check how they react
                if (mFactor < 20 && playHand == CardValuationType.Risky)
                {
                    return PlayerAction.Raise(context.CurrentMaxBet);
                }

                if (playHand == CardValuationType.Unplayable)
                {
                    if (context.CanCheck)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }
                
                // raises risky hands only if in position and if it is not a 3betted Pot
                if (playHand == CardValuationType.Risky && context.MyMoneyInTheRound == context.SmallBlind)
                {
                 //   var smallBlindsTimes = RandomProvider.Next(1, 8);
                    return PlayerAction.Raise(context.SmallBlind * 6);
                }
                // folds if not in position
                else if(playHand == CardValuationType.Risky && context.MyMoneyInTheRound != context.SmallBlind)
                {
                    
                    if (context.CanCheck)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationType.Recommended)
                {
                   // var smallBlindsTimes = RandomProvider.Next(6, 10);
                    return PlayerAction.Raise(context.SmallBlind * 6);
                }

                return PlayerAction.CheckOrCall();
            }
            #endregion

            // This doesn't make a difference if it is FLOP, TURN or RIVER!
            #region Post-FlopLogic 
            //always call SmallBlinds!
            if (context.MoneyToCall == context.SmallBlind)
            {
                return PlayerAction.CheckOrCall();
            }
          
            int ourCurrentStack = context.MoneyLeft;
            //if the pot is bigger than our money= ALL-IN BABY!
            if (context.CurrentPot > context.MoneyLeft && context.MoneyLeft > 0)
            {
                return PlayerAction.Raise(ourCurrentStack);
            }
            
            var chanceForAction = RandomProvider.Next(1, 50);

            double currentPotRaise = context.CurrentPot * 0.55;
            int currentPotRaiseInt = (int)currentPotRaise;

            if (chanceForAction <= 35)
            {
                return PlayerAction.Raise(currentPotRaiseInt);
            }

            if (chanceForAction > 35)
            {
                return PlayerAction.CheckOrCall();
            }
           
            if (context.CanCheck)
            {
                return PlayerAction.CheckOrCall();
            }

            else
            {
                return PlayerAction.Fold();
            }
        }
        #endregion

    }
}