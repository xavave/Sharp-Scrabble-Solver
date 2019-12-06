using System;
using System.Collections.Generic;
using Scrabble2018.Model;
using Scrabble2018.Model.Word;
using Scrabble2018.View;
using System.Windows.Media;
using DawgResolver;

namespace Scrabble2018.Controller
{
    public class GameGUI
    {
        private Random rnd = new Random();
        public int PlayerFirst, PlayerNow;
        //public AllTiles TilesBag;
        public bool BoardChanged;
        public GameState gs;
        public MoveRecorder moveRecorder;
        public List<char> ListSwap;

        public GameGUI()
        {

            moveRecorder = new MoveRecorder();
            this.gs = GameState.GSInstance;
            this.ListSwap = new List<char>();
            this.BoardChanged = false;
        }


        //public bool Validate(char[,] bc)
        //{
        //    if (MoveValidator.Validate(gs, bc, moveRecorder) != -1 && !GameEnd())
        //    {
        //        return true;
        //    }
        //    else return false;
        //}

        public void Subs(IView view)
        {
            if (view != null) gs.ListOfViews.Add(view);
        }

        public void Unsubs(IView view)
        {
            if (view != null) gs.ListOfViews.Remove(view);
        }

        //public void GetNewTiles(List<char> Loc, int num)
        //{
        //    gs.playerManager.GetNewTiles(gs, Loc, num);
        //}

        public void UpdateState(char[,] b)
        {
            gs.UpdateState(b);
        }

        //public bool CanSwap()
        //{
        //    return gs.TilesBag.ListTiles.Count >= 7;
        //}

        //public char SwapChar(char c)
        //{
        //    return GameState.GSInstance.playerManager.Swap(c);
        //}

        //public bool GameEnd()
        //{
        //    return GameEndVerify.GameEndScoring(gs);
        //}
        public void ClearMovement()
        {
            moveRecorder.Moves.Clear();
        }

       

    }

}
