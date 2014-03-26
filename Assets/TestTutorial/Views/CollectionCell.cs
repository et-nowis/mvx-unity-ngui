using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;
using Cirrious.MvvmCross.Binding.Unity.Views;
using UnityEngine;
using TestTutorial.Models;


public class CollectionCell : MvxCollectionViewCell 
{
    //public UIButton clickButton;
    public UILabel cellLabel;
	public UISprite cellSprite;
	public UITexture cellTexture;

	private MvxImageViewLoader _loader;

	protected override void Awake()
    {
		base.Awake();

		_loader = new MvxImageViewLoader( () => cellTexture );

		this.DelayBind(() => {

	        var bindings = this.CreateBindingSet<CollectionCell, Cell>();
	        	bindings.Bind(cellLabel).To(item => item.CellValue);
				//bindings.Bind(cellSprite).To(item => item.CellSpriteName);

				bindings.Bind(_loader).For("ImageUrl").To(item => item.CellTextureName);
	       		//bindings.Bind(clickButton).To(vm => vm.ClickCommand);
	        bindings.Apply();
			
		});
		
    }

}


