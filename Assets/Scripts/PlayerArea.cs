﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CasinoGames.Core
{
    public class PlayerArea : MonoBehaviour
    {
        [SerializeField]
        private IEnumerable<Chipstack> chipStacks;
        private Cardstack cardstack;

        public void AddCard(ICard card)
        {
            throw new NotImplementedException();
        }

        public void UpdateCard(int cardIndex, ICard cardDetails)
        {
            throw new NotImplementedException();
        }

        public void RemoveCards()
        {
            throw new NotImplementedException();
        }

        public void SetChips(IEnumerable<CasinoChip> chips)
        {
            throw new NotImplementedException();
        }

        private Chipstack GetEmptyChipStack()
        {
            throw new NotImplementedException();
        }
    }
}