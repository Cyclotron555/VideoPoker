using System;

namespace Poker
{
    class CardDefinitions
    {

        public int ReturnCardKind(int cCardKind)
        {
            switch (cCardKind)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 1;
                case 15: return 2;
                case 16: return 3;
                case 17: return 4;
                case 18: return 5;
                case 19: return 6;
                case 20: return 7;
                case 21: return 8;
                case 22: return 9;
                case 23: return 10;
                case 24: return 11;
                case 25: return 12;
                case 26: return 13;
                case 27: return 1;
                case 28: return 2;
                case 29: return 3;
                case 30: return 4;
                case 31: return 5;
                case 32: return 6;
                case 33: return 7;
                case 34: return 8;
                case 35: return 9;
                case 36: return 10;
                case 37: return 11;
                case 38: return 12;
                case 39: return 13;
                case 40: return 1;
                case 41: return 2;
                case 42: return 3;
                case 43: return 4;
                case 44: return 5;
                case 45: return 6;
                case 46: return 7;
                case 47: return 8;
                case 48: return 9;
                case 49: return 10;
                case 50: return 11;
                case 51: return 12;
                case 52: return 13;
            }
            return cCardKind;
        }
        public string ReturnCardColor(int cCardColor)
        {

            switch (cCardColor)
            {
                case 0: return "joker";
                case 1: return "hearts";
                case 2: return "hearts";
                case 3: return "hearts";
                case 4: return "hearts";
                case 5: return "hearts";
                case 6: return "hearts";
                case 7: return "hearts";
                case 8: return "hearts";
                case 9: return "hearts";
                case 10: return "hearts";
                case 11: return "hearts";
                case 12: return "hearts";
                case 13: return "hearts";
                case 14: return "diamonds";
                case 15: return "diamonds";
                case 16: return "diamonds";
                case 17: return "diamonds";
                case 18: return "diamonds";
                case 19: return "diamonds";
                case 20: return "diamonds";
                case 21: return "diamonds";
                case 22: return "diamonds";
                case 23: return "diamonds";
                case 24: return "diamonds";
                case 25: return "diamonds";
                case 26: return "diamonds";
                case 27: return "spades";
                case 28: return "spades";
                case 29: return "spades";
                case 30: return "spades";
                case 31: return "spades";
                case 32: return "spades";
                case 33: return "spades";
                case 34: return "spades";
                case 35: return "spades";
                case 36: return "spades";
                case 37: return "spades";
                case 38: return "spades";
                case 39: return "spades";
                case 40: return "clubs";
                case 41: return "clubs";
                case 42: return "clubs";
                case 43: return "clubs";
                case 44: return "clubs";
                case 45: return "clubs";
                case 46: return "clubs";
                case 47: return "clubs";
                case 48: return "clubs";
                case 49: return "clubs";
                case 50: return "clubs";
                case 51: return "clubs";
                case 52: return "clubs";
            }
            return Convert.ToString(cCardColor);
        }
        public string ReturnCardType(int cCardType)
        {

            switch (cCardType)
            {
                case 0: return "jk";
                case 1: return "ace";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                case 5: return "five";
                case 6: return "six";
                case 7: return "seven";
                case 8: return "eight";
                case 9: return "nine";
                case 10: return "ten";
                case 11: return "jack";
                case 12: return "queen";
                case 13: return "king";
                case 14: return "ace";
                case 15: return "two";
                case 16: return "three";
                case 17: return "four";
                case 18: return "five";
                case 19: return "six";
                case 20: return "seven";
                case 21: return "eight";
                case 22: return "nine";
                case 23: return "ten";
                case 24: return "jack";
                case 25: return "queen";
                case 26: return "king";
                case 27: return "ace";
                case 28: return "two";
                case 29: return "three";
                case 30: return "four";
                case 31: return "five";
                case 32: return "six";
                case 33: return "seven";
                case 34: return "eight";
                case 35: return "nine";
                case 36: return "ten";
                case 37: return "jack";
                case 38: return "queen";
                case 39: return "king";
                case 40: return "ace";
                case 41: return "two";
                case 42: return "three";
                case 43: return "four";
                case 44: return "five";
                case 45: return "six";
                case 46: return "seven";
                case 47: return "eight";
                case 48: return "nine";
                case 49: return "ten";
                case 50: return "jack";
                case 51: return "queen";
                case 52: return "king";
            }
            return Convert.ToString(cCardType);
        }
    }
}
