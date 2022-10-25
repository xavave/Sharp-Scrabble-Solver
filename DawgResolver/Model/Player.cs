﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DawgResolver.Model
{
    public class Player
    {
        public string Name { get; set; }
        Game Game { get; set; }
        public PlayerRack Rack { get; }
        public int Points { get; set; }
        public HashSet<Word> Moves { get; }
        public Player(Game g, string name)
        {
            Game = g;
            Name = name;
            Rack = new PlayerRack();
            Moves = new HashSet<Word>();

        }


        public void SetRackFromWord(string word)
        {
            Rack.Clear();
            Rack.AddRange(word.Select(s => new Letter( s, 1, 1)));
        }

    }
    public class PlayerRack : ICloneable
    {
        public List<Letter> Letters { get; set; }
        public PlayerRack()
        {
            Letters = new List<Letter>(7);
        }
        public Letter this[int index]
        {
            get => Letters[index];
            set => Letters[index] = value;
        }
        public void Clear()
        {
            Letters.Clear();
        }
        public void AddRange(IEnumerable<Letter> letters)
        {
            Letters.AddRange(letters);
        }

        internal int Count() => Letters.Count;
        public override string ToString() => string.Join("", Letters.Select(s => s.Char));

        public bool Any() => Letters.Any();
        public void Remove(Letter letter) => Letters.RemoveLetterByIndex(letter);

        public PlayerRack Backup()
        {
            return this.Clone() as PlayerRack;
        }
        public PlayerRack Restore(PlayerRack playerRack)
        {
            this.Letters = playerRack.Letters;
            return this;
        }
        public object Clone()
        {
            return new PlayerRack()
            {
                Letters = new List<Letter>(this.Letters),
            };
        }
        internal void Add(Letter letter) => Letters.Add(letter);

    }
}
