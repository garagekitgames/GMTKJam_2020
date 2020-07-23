using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
    /// <summary>
    /// A debug menu helper, meant to help create quick mobile friendly debug menus
    /// </summary>
    public class MMDebugMenu : MonoBehaviour
    {
        /// the possible directions for the menu to appear
        public enum ToggleDirections { TopToBottom, LeftToRight, RightToLeft, BottomToTop }

        [Header("Bindings")]
        /// the container of the whole menu
        public CanvasGroup MenuContainer;
        /// the scrolling contents
        public RectTransform Contents;
        /// the scriptable object containing the menu's data
        public MMDebugMenuData Data;
        /// the menu's background image
        public Image MenuBackground;
        /// the icon used to close the menu
        public Image CloseIcon;

        [Header("Test")]
        /// whether or not this menu is active at this moment
        [MMReadOnly]
        public bool Active = false;
        /// a test button to toggle the menu
        [MMInspectorButton("ToggleMenu")]
        public bool ToggleButton;

        protected RectTransform _containerRect;
        protected Vector3 _initialContainerPosition;
        protected Vector3 _offPosition;
        protected Vector3 _newPosition;
        protected WaitForSeconds _toggleWFS;
        protected bool _toggling = false;

        /// <summary>
        /// On Start we init our menu
        /// </summary>
        protected virtual void Start()
        {
            Initialization();
        }

        /// <summary>
        /// Prepares transitions and grabs components
        /// </summary>
        protected virtual void Initialization()
        {
            CloseIcon.color = Data.TextColor;
            _toggleWFS = new WaitForSeconds(Data.ToggleDuration);
            _containerRect = MenuContainer.GetComponent<RectTransform>();
            _initialContainerPosition = _containerRect.localPosition;
            MenuBackground.color = Data.BackgroundColor;
            switch (Data.ToggleDirection)
            {
                case ToggleDirections.RightToLeft:
                    _offPosition = _initialContainerPosition + Vector3.right * _containerRect.rect.width;
                    break;
                case ToggleDirections.LeftToRight:
                    _offPosition = _initialContainerPosition + Vector3.left * _containerRect.rect.width;
                    break;
                case ToggleDirections.TopToBottom:
                    _offPosition = _initialContainerPosition + Vector3.up * _containerRect.rect.height;
                    break;
                case ToggleDirections.BottomToTop:
                    _offPosition = _initialContainerPosition + Vector3.down * _containerRect.rect.height;
                    break;
            }
            
            _containerRect.localPosition = _offPosition;

            if (Data != null)
            {
                FillMenu();
            }
        }

        /// <summary>
        /// Fills the menu based on the data's contents
        /// </summary>
        public virtual void FillMenu()
        {
            foreach (Transform child in Contents.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (MMDebugMenuItem item in Data.MenuItems)
            {
                switch (item.Type)
                {
                    case MMDebugMenuItem.MMDebugMenuItemTypes.Button:
                        MMDebugMenuItemButton button;
                        button = (item.ButtonType == MMDebugMenuItem.MMDebugMenuItemButtonTypes.Border) ? Instantiate(Data.ButtonBorderPrefab) : Instantiate(Data.ButtonPrefab);
                        button.ButtonText.text = item.ButtonText;
                        button.ButtonEventName = item.ButtonEventName;
                        if (item.ButtonType == MMDebugMenuItem.MMDebugMenuItemButtonTypes.Border)
                        {
                            button.ButtonText.color = Data.AccentColor;
                            button.ButtonBg.color = Data.TextColor;
                        }
                        else
                        {
                            button.ButtonText.color = Data.BackgroundColor;
                            button.ButtonBg.color = Data.AccentColor;
                        }
                        button.ButtonText.font = Data.RegularFont;
                        button.transform.SetParent(Contents.transform);
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Checkbox:
                        MMDebugMenuItemCheckbox checkbox = Instantiate(Data.CheckboxPrefab);
                        checkbox.SwitchText.text = item.CheckboxText;
                        if (item.CheckboxInitialState)
                        {
                            checkbox.Switch.SetTrue();
                        }
                        else
                        {
                            checkbox.Switch.SetFalse();
                        }
                        checkbox.CheckboxEventName = item.CheckboxEventName;                        
                        checkbox.transform.SetParent(Contents.transform);
                        checkbox.Switch.GetComponent<Image>().color = Data.AccentColor;
                        checkbox.SwitchText.color = Data.TextColor;
                        checkbox.SwitchText.font = Data.RegularFont;
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Slider:
                        MMDebugMenuItemSlider slider = Instantiate(Data.SliderPrefab);
                        slider.Mode = item.SliderMode;
                        slider.RemapZero = item.SliderRemapZero;
                        slider.RemapOne = item.SliderRemapOne;
                        slider.TargetSlider.value = MMMaths.Remap(item.SliderInitialValue, item.SliderRemapZero, item.SliderRemapOne, 0f, 1f);
                        slider.transform.SetParent(Contents.transform);

                        slider.SliderText.text = item.SliderText;
                        slider.SliderText.color = Data.TextColor;
                        slider.SliderText.font = Data.RegularFont;

                        slider.SliderValueText.text = (item.SliderMode == MMDebugMenuItemSlider.Modes.Int) ? item.SliderInitialValue.ToString() : item.SliderInitialValue.ToString("F3"); 
                        slider.SliderValueText.color = Data.AccentColor;
                        slider.SliderValueText.font = Data.BoldFont;

                        slider.SliderKnob.color = Data.AccentColor;
                        slider.SliderLine.color = Data.TextColor;

                        slider.SliderEventName = item.SliderEventName;
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Spacer:
                        GameObject spacerPrefab = (item.SpacerType == MMDebugMenuItem.MMDebugMenuItemSpacerTypes.Small) ? Data.SpacerSmallPrefab : Data.SpacerBigPrefab;
                        GameObject spacer = Instantiate(spacerPrefab);
                        spacer.transform.SetParent(Contents.transform);
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Title:
                        MMDebugMenuItemTitle title = Instantiate(Data.TitlePrefab);
                        title.TitleText.text = item.TitleText;
                        title.TitleText.color = Data.TextColor;
                        title.TitleText.font = Data.BoldFont;
                        title.TitleLine.color = Data.AccentColor;
                        title.transform.SetParent(Contents.transform);
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Choices:
                        MMDebugMenuItemChoices choicesPrefab;
                        if (item.ChoicesType == MMDebugMenuItem.MMDebugMenuItemChoicesTypes.TwoChoices)
                        {
                            choicesPrefab = Data.TwoChoicesPrefab;
                        }
                        else
                        {
                            choicesPrefab = Data.ThreeChoicesPrefab;
                        }

                        MMDebugMenuItemChoices choices = Instantiate(choicesPrefab);

                        choices.Choices[0].ButtonText.text = item.ChoiceOneText;
                        choices.Choices[1].ButtonText.text = item.ChoiceTwoText;

                        choices.Choices[0].ButtonEventName = item.ChoiceOneEventName;
                        choices.Choices[1].ButtonEventName = item.ChoiceTwoEventName;

                        if (item.ChoicesType == MMDebugMenuItem.MMDebugMenuItemChoicesTypes.ThreeChoices)
                        {
                            choices.Choices[2].ButtonEventName = item.ChoiceThreeEventName;
                            choices.Choices[2].ButtonText.text = item.ChoiceThreeText;
                        }

                        choices.OffColor = Data.BackgroundColor;
                        choices.OnColor = Data.TextColor;
                        choices.AccentColor = Data.AccentColor;

                        foreach(MMDebugMenuChoiceEntry entry in choices.Choices)
                        {
                            if (entry != null)
                            {
                                entry.ButtonText.font = Data.RegularFont;
                            }
                        }

                        choices.Select(item.SelectedChoice);
                        choices.transform.SetParent(Contents.transform);
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Value:
                        MMDebugMenuItemValue value = Instantiate(Data.ValuePrefab);
                        value.LabelText.text = item.ValueLabel;
                        value.LabelText.color = Data.TextColor;
                        value.LabelText.font = Data.RegularFont;
                        value.ValueText.text = item.ValueInitialValue;
                        value.ValueText.color = Data.AccentColor;
                        value.ValueText.font = Data.BoldFont;
                        value.RadioReceiver.Channel = item.ValueMMRadioReceiverChannel;
                        value.transform.SetParent(Contents.transform);
                        break;

                    case MMDebugMenuItem.MMDebugMenuItemTypes.Text:

                        MMDebugMenuItemText textPrefab;
                        switch (item.TextType)
                        {
                            case MMDebugMenuItem.MMDebugMenuItemTextTypes.Tiny:
                                textPrefab = Data.TextTinyPrefab;
                                break;
                            case MMDebugMenuItem.MMDebugMenuItemTextTypes.Small:
                                textPrefab = Data.TextSmallPrefab;
                                break;
                            case MMDebugMenuItem.MMDebugMenuItemTextTypes.Long:
                                textPrefab = Data.TextLongPrefab;
                                break;
                            default:
                                textPrefab = Data.TextTinyPrefab;
                                break;
                        }
                        MMDebugMenuItemText text = Instantiate(textPrefab);
                        text.ContentText.text = item.TextContents;
                        text.ContentText.color = Data.TextColor;
                        text.ContentText.font = Data.RegularFont;
                        text.transform.SetParent(Contents.transform);
                        break;
                }
            }

            // we always add a spacer at the end because scrollviews are terrible
            GameObject finalSpacer = Instantiate(Data.SpacerBigPrefab);
            finalSpacer.transform.SetParent(Contents.transform);
        }

        /// <summary>
        /// Makes the menu appear
        /// </summary>
        public virtual void OpenMenu()
        {
            StartCoroutine(ToggleCo(false));
        }

        /// <summary>
        /// Makes the menu disappear
        /// </summary>
        public virtual void CloseMenu()
        {
            StartCoroutine(ToggleCo(true));
        }

        /// <summary>
        /// Closes or opens the menu depending on its current state
        /// </summary>
        public virtual void ToggleMenu()
        {
            StartCoroutine(ToggleCo(Active));
        }

        /// <summary>
        /// A coroutine used to toggle the menu
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        protected virtual IEnumerator ToggleCo(bool active)
        {
            if (_toggling)
            {
                yield break;
            }
            if (!active)
            {
                _containerRect.gameObject.SetActive(true);
            }
            _toggling = true;
            Active = active;
            _newPosition = active ? _offPosition : _initialContainerPosition;
            MMTween.MoveRectTransform(this, _containerRect, _containerRect.localPosition, _newPosition, null, 0f, Data.ToggleDuration, Data.ToggleCurve);
            yield return _toggleWFS;
            if (active)
            {
                _containerRect.gameObject.SetActive(false);
            }
            Active = !active;
            _toggling = false;
        }

        /// <summary>
        /// On update we handle our input
        /// </summary>
        protected virtual void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// Looks for shortcut input
        /// </summary>
        protected virtual void HandleInput()
        {
            if (Input.GetKeyDown(Data.ToggleShortcut))
            {
                ToggleMenu();
            }
        }
    }

}
