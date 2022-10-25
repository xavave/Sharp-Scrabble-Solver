using System.Drawing;
using System.Windows.Forms;

using Dawg.Resolver.Winform.Test;
using Dawg.Scrabble.Model.Models;
using Dawg.Scrabble.Model.Repository;

namespace Dawg.Presenter
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IMainRepository _repository;

        public MainPresenter(IMainView view,IMainRepository repository )
        {
            _view = view;
            view.Presenter = this;
            _repository = repository;
        }

        public int PreviewWord(Player p, Word word, bool validateWord = false, bool addMove = true)
        {
            //ClearTilesInPlay(p);
            int points = word.SetWord(validateWord);
            if (validateWord || points > 0)
            {
                if (addMove)
                {
                    word.Index = game.MoveIndex++;
                    word.IsPlayedByPlayer1 = p.Name == game.Player1.Name;
                    p.Moves.Add(word);
                    game.Resolver.PlayedWords.Add(word);
                    DisplayPlayerWord(word);
                }
                foreach (var t in word.GetTiles())
                {
                    var frmTile = FindFormTile(t);
                    if (frmTile.IsEmpty)
                    {
                        frmTile.Invoke((MethodInvoker)(() =>
                        {
                            frmTile.SetBackColorFromInnerTile();

                        }));
                    }
                    else
                    {
                        if (t.WordIndex == 0)
                        {
                            //frmTile.IsPlayedByPlayer1 = p == game.Player1;
                            t.WordIndex = word.Index;
                            frmTile.Invoke((MethodInvoker)(() =>
                            {
                                if (t.IsPlayedByPlayer1.HasValue)
                                    frmTile.BackColor = t.IsPlayedByPlayer1.Value ? Player1MoveColor : Player2MoveColor;
                            }));
                        }
                    }

                    if (frmTile.IsValidated)
                        continue;

                    if (t.Letter.LetterType == LetterType.Joker)
                    {
                        if (frmTile.InvokeRequired) frmTile.Invoke((MethodInvoker)(() => frmTile.SetBackColorFromInnerLetterType()));
                        else frmTile.BackColor = Color.Gold;

                        game.CurrentPlayer.Rack.Remove(game.Resolver.Alphabet[26]);
                    }

                    else
                    {
                        game.CurrentPlayer.Rack.Remove(t.Letter);
                    }
                }

                if (validateWord)
                    game.IsPlayer1 = !game.IsPlayer1;

            }
            else return 0;

            RefreshBoard();
            return points;
        }
    }

   
}
