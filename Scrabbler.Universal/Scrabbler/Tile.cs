#define FIX_UWP_DOUBLE_TAPS   // Double-taps don't work well on UWP as of 2.3.0
#define FIX_UWP_NULL_CONTENT  // Set Content of Frame to null doesn't work in UWP as of 2.3.0

using DawgResolver.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace Scrabbler
{
    public enum TileStatus
    {
        Hidden,
        Flagged,
        Exposed
    }

    public class Tile : Frame, VTile
    {
        TileStatus tileStatus = TileStatus.Exposed;
        public Label label;
        Image flagImage, bugImage;
        static ImageSource flagImageSource;
        static ImageSource bugImageSource;
        bool doNotFireEvent;

        public event EventHandler<TileStatus> TileStatusChanged;
        public static Assembly Assembly { get => Assembly.GetExecutingAssembly(); }
        static Tile()
        {
            flagImageSource = ImageSource.FromResource("Scrabbler.Images.Xamarin120.png", Assembly);
            bugImageSource = ImageSource.FromResource("Scrabbler.Images.RedBug.png", Assembly);
        }
        public VTile InnerTile { get; }
        public Tile(VTile vti)
        {
            InnerTile = vti;
            this.Row = vti.Ligne;
            this.Col = vti.Col;
            this.ClassId = $"t{vti.Ligne}_{vti.Col}";
            this.BackgroundColor = GetTileColor(vti);
            this.BorderColor = Color.DarkGray;
            this.Padding = 2;

            label = new Label
            {
                Text = GetTileLabel(vti),
                TextColor = Color.Black,
                BackgroundColor = GetTileColor(vti),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            this.Content = label;
            flagImage = new Image
            {
                Source = flagImageSource,

            };

            bugImage = new Image
            {
                Source = bugImageSource
            };

            TapGestureRecognizer singleTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1
            };
            singleTap.Tapped += OnSingleTap;
            this.GestureRecognizers.Add(singleTap);

#if FIX_UWP_DOUBLE_TAPS

            if (Device.RuntimePlatform != Device.UWP)
            {

#endif

                TapGestureRecognizer doubleTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };
                doubleTap.Tapped += OnDoubleTap;
                this.GestureRecognizers.Add(doubleTap);

#if FIX_UWP_DOUBLE_TAPS

            }

#endif

        }

        public Color GetTileColor(VTile vti)
        {
            switch (vti.TileType)
            {
                case TileType.Center: return Color.Yellow;
                case TileType.DoubleLetter: return Color.LightBlue;
                case TileType.TripleLetter: return Color.Blue;
                case TileType.DoubleWord: return Color.Orange;
                case TileType.TripleWord: return Color.Red;
                default: return Color.White;
            }
        }

        private string GetTileLabel(VTile vti)
        {
            switch (vti.TileType)
            {
                case TileType.Center: return "C";
                case TileType.DoubleLetter: return "2L";
                case TileType.TripleLetter: return "3L";
                case TileType.DoubleWord: return "2W";
                case TileType.TripleWord: return "3W";
                default: return "";
            }
        }

        public int Row { private set; get; }

        public int Col { private set; get; }

        public bool IsBug { set; get; }

        public int SurroundingBugCount { set; get; }

        public TileStatus Status
        {
            set
            {
                //if (tileStatus != value)
                //{
                    tileStatus = value;

                    switch (tileStatus)
                    {
                        case TileStatus.Hidden:
                            this.Content = null;

#if FIX_UWP_NULL_CONTENT

                            if (Device.RuntimePlatform == Device.UWP)
                            {
                                this.Content = new Label { Text = " " };
                            }

#endif
                            break;

                        case TileStatus.Flagged:
                            this.Content = flagImage;
                            break;

                        case TileStatus.Exposed:
                            if (this.IsBug)
                            {
                                this.Content = bugImage;
                            }
                            else
                        {

                            //label.Text = this.InnerTile.Letter?.Char.ToString();
                            this.Content = label;
                               
                                //label.Text =
                                //        (this.SurroundingBugCount > 0) ?
                                //            this.SurroundingBugCount.ToString() : " ";
                            }
                            break;
                    }

                    if (!doNotFireEvent && TileStatusChanged != null)
                    {
                        TileStatusChanged(this, tileStatus);
                    }
                //}
            }
            get
            {
                return tileStatus;
            }
        }

        public bool IsValidated { get => InnerTile.IsValidated; set => InnerTile.IsValidated=value; }

        public TileType TileType => InnerTile.TileType;

        public int Ligne { get => InnerTile.Ligne; set => InnerTile.Ligne=value; }
        int VTile.Col { get => InnerTile.Col; set => InnerTile.Col=value; }
        public Letter Letter { get => InnerTile.Letter; set => InnerTile.Letter=value; }
        public int LetterMultiplier { get => InnerTile.LetterMultiplier; set => InnerTile.LetterMultiplier = value; }
        public int WordMultiplier { get => InnerTile.WordMultiplier; set => InnerTile.WordMultiplier = value; }
        public int AnchorLeftMinLimit { get => InnerTile.AnchorLeftMinLimit; set => InnerTile.AnchorLeftMinLimit = value; }
        public int AnchorLeftMaxLimit { get => InnerTile.AnchorLeftMaxLimit; set => InnerTile.AnchorLeftMaxLimit = value; }

        public bool IsAnchor => InnerTile.IsAnchor;

        public Dictionary<int, int> Controlers { get => InnerTile.Controlers; set => InnerTile.Controlers = value; }
        public bool FromJoker { get => InnerTile.FromJoker; set => InnerTile.FromJoker = value; }

        public bool IsEmpty => InnerTile.IsEmpty;

        public bool? IsPlayedByPlayer1 { get => InnerTile.IsPlayedByPlayer1; set => InnerTile.IsPlayedByPlayer1 = value; }

        public VTile LeftTile => InnerTile.LeftTile;

        public VTile RightTile => InnerTile.RightTile;

        public VTile UpTile => InnerTile.UpTile;

        public VTile DownTile => InnerTile.DownTile;

        public string Serialize
        {
            get
            {
                return $"T{Ligne};{Col};{LetterMultiplier};{WordMultiplier};{FromJoker};{IsValidated};{Letter?.Char};{IsPlayedByPlayer1}";
            }
        }

        public VTile WordMostRightTile => InnerTile.WordMostRightTile;

        public VTile WordMostLeftTile => InnerTile.WordMostLeftTile;

        public VTile WordLowerTile => InnerTile.WordLowerTile;

        public VTile WordUpperTile => InnerTile.WordUpperTile;

        public int WordIndex { get => InnerTile.WordIndex; set => InnerTile.WordIndex = value; }

        // Does not fire TileStatusChanged events.
        public void Initialize()
        {
            doNotFireEvent = true;
            this.Status = TileStatus.Exposed;
            this.IsBug = false;
            this.SurroundingBugCount = 0;
            doNotFireEvent = false;
        }

#if FIX_UWP_DOUBLE_TAPS

        bool lastTapSingle;
        DateTime lastTapTime;

#endif

        void OnSingleTap(object sender, object args)
        {

#if FIX_UWP_DOUBLE_TAPS

            if (Device.RuntimePlatform == Device.UWP)
            {
                if (lastTapSingle && DateTime.Now - lastTapTime < TimeSpan.FromMilliseconds(500))
                {
                    OnDoubleTap(sender, args);
                    lastTapSingle = false;
                }
                else
                {
                    lastTapTime = DateTime.Now;
                    lastTapSingle = true;
                }
            }

#endif

            switch (this.Status)
            {
                case TileStatus.Hidden:
                    this.Status = TileStatus.Flagged;
                    break;

                case TileStatus.Flagged:
                    this.Status = TileStatus.Hidden;
                    break;

                case TileStatus.Exposed:
                    // Do nothing
                    break;
            }
        }

        void OnDoubleTap(object sender, object args)
        {
            this.Status = TileStatus.Exposed;
        }

        public Word GetWordFromTile(MovementDirection direction)
        {
            throw new NotImplementedException();
        }


    }
}
