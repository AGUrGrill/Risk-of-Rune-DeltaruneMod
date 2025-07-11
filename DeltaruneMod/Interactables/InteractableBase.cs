﻿using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DeltaruneMod.Interactables
{
    public abstract class InteractableBase<T> : InteractableBase where T : InteractableBase<T>
    {
        public static T instance { get; private set; }

        public InteractableBase()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting PurchaseInteractableBase/Interactable was instantiated twice");
            instance = this as T;
        }
    }

    public abstract class InteractableBase
    {
        public abstract string InteractableName { get; }

        public abstract string InteractableContext { get; }

        public abstract string InteractableLangToken { get; }

        public abstract GameObject InteractableModel { get; }

        public abstract void Init();

        protected void CreateLang()
        {
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_NAME", InteractableName);
            LanguageAPI.Add("INTERACTABLE_" + InteractableLangToken + "_CONTEXT", InteractableContext);
        }
    }
}
