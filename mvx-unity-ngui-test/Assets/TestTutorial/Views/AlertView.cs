using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;
using UnityEngine;
using Cirrious.MvvmCross.Binding.Attributes;

[MvxView("TestTutorial/Views/AlertView")]
public class AlertView : BaseViewController, IMvxModalUnityView
{
    public UILabel messageLabel;
    public UILabel captionLabel;

    public Transform ButtonGroup;
    public Transform UIIconGroup;
    public Transform ItemIconGroup;

    public GameObject ButtonPrefab;
    public GameObject UIIconPrefab;
    public GameObject ItemIconPrefab;

    private List<UIButton> buttons;
    private List<GameObject> uiicons;
    private List<GameObject> itemicons;

    public UIButton CloseButton;

    public UIButton ConfirmButton;
    public UILabel ConfirmActionLabel;
    public UISprite ConfirmButtonIcon;
    public UILabel ConfirmAmountLabel;

    public new AlertViewModel ViewModel
    {
        get { return (AlertViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var bindingSet = this.CreateBindingSet<AlertView, AlertViewModel>();
        bindingSet.Bind(ConfirmButton).To(vm => vm.ConfirmButtonClickCommand);
        bindingSet.Bind(CloseButton).To(vm => vm.CloseCommand);
        bindingSet.Apply();


        messageLabel.text = this.ViewModel.Message;
        captionLabel.text = this.ViewModel.Title;

        if (this.ViewModel.ButtonTexts[0] == "")
            NGUITools.SetActive(ButtonGroup.gameObject, false);
        else
        {
            buttons = new List<UIButton>();

            Bounds buttonBound = new Bounds(Vector3.zero, Vector3.zero);
            int i = 0;
            foreach (string title in this.ViewModel.ButtonTexts)
            {
                UIButton button = NGUITools.AddChild(this.ButtonGroup.gameObject, ButtonPrefab).GetComponent<UIButton>();
                button.transform.localPosition = new Vector3(i * 60f, 0, 0);
                button.name = string.Format("{0}", i);
                button.GetComponentInChildren<UILabel>().text = title;

                UIEventListener.Get(button.gameObject).onClick = OnButtonClick;

                buttons.Add(button);
                buttonBound = NGUIMath.CalculateRelativeWidgetBounds(button.transform);

                i++;
            }

            UIGrid buttonGrid = ButtonGroup.GetComponent<UIGrid>();

            buttonGrid.cellWidth = buttonBound.size.x + 10f;
            buttonGrid.cellHeight = buttonBound.size.y;

            ButtonGroup.transform.localPosition = new Vector3(buttonGrid.transform.localPosition.x - (i - 1) * buttonGrid.cellWidth / 2,
                buttonGrid.transform.localPosition.y, buttonGrid.transform.localPosition.z);
            buttonGrid.Reposition();
        }


        //ui icons
        if (this.ViewModel.UIIconNames[0] == "")
            NGUITools.SetActive(UIIconGroup.gameObject, false);
        else
        {
            uiicons = new List<GameObject>();

            Bounds iconBound = new Bounds(Vector3.zero, Vector3.zero);
            int i = 0;
            foreach (string name in this.ViewModel.UIIconNames)
            {
                GameObject icon = NGUITools.AddChild(this.UIIconGroup.gameObject, UIIconPrefab);
                icon.transform.localPosition = new Vector3(i * 60f, 0, 0);
                icon.name = string.Format("{0}", i);

                icon.GetComponentInChildren<UISprite>().spriteName = name;
                icon.GetComponentInChildren<UILabel>().text = this.ViewModel.UIIconTexts[i];

                uiicons.Add(icon);
                iconBound = NGUIMath.CalculateRelativeWidgetBounds(icon.transform);

                i++;
            }

            UIGrid iconGrid = UIIconGroup.GetComponent<UIGrid>();
            iconGrid.cellWidth = iconBound.size.x + 5f;
            iconGrid.cellHeight = iconBound.size.y;

            UIIconGroup.transform.localPosition = new Vector3(iconGrid.transform.localPosition.x - (i - 1) * iconGrid.cellWidth / 2,
                iconGrid.transform.localPosition.y, iconGrid.transform.localPosition.z);

            iconGrid.Reposition();
        }

        //item icons
        if (this.ViewModel.ItemIconNames[0] == "")
            NGUITools.SetActive(ItemIconGroup.gameObject, false);
        else
        {
            itemicons = new List<GameObject>();

            Bounds iconBound = new Bounds(Vector3.zero, Vector3.zero);
            int i = 0;
            foreach (string name in this.ViewModel.ItemIconNames)
            {
                if (name != "")
                {
                    GameObject icon = NGUITools.AddChild(this.ItemIconGroup.gameObject, ItemIconPrefab);
                    icon.transform.localPosition = new Vector3(i * 70f, 0, 0);
                    icon.name = string.Format("{0}", i);

                    icon.GetComponentInChildren<UISprite>().spriteName = name;
                    icon.GetComponentInChildren<UILabel>().text = this.ViewModel.ItemIconTexts[i];

                    itemicons.Add(icon);
                    iconBound = NGUIMath.CalculateRelativeWidgetBounds(icon.transform);

                    i++;
                }
            }

            UIGrid iconGrid = ItemIconGroup.GetComponent<UIGrid>();
            iconGrid.cellWidth = iconBound.size.x + 5f;
            iconGrid.cellHeight = iconBound.size.y;

            ItemIconGroup.transform.localPosition = new Vector3(iconGrid.transform.localPosition.x - (i - 1) * iconGrid.cellWidth / 2,
                iconGrid.transform.localPosition.y, iconGrid.transform.localPosition.z);

            iconGrid.Reposition();
        }

        UpdateConfirmButton();
    }

    public void OnButtonClick(GameObject go)
    {
        int buttonIndex = int.Parse(go.name);
        this.ViewModel.ButtonClickCommand.Execute(buttonIndex);
    }

    private void UpdateConfirmButton()
    {
        if (this.ViewModel.ConfirmButtonTexts.Count == 3)
        {
            this.ConfirmActionLabel.text = this.ViewModel.ConfirmButtonTexts[0];
            this.ConfirmButtonIcon.spriteName = this.ViewModel.ConfirmButtonTexts[1];
            this.ConfirmAmountLabel.text = this.ViewModel.ConfirmButtonTexts[2];
        }
        else
        {
            NGUITools.SetActive(this.ConfirmButton.gameObject, false);
        }
    }
}


