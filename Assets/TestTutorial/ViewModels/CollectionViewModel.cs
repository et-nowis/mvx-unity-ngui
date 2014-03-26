using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.ViewModels;
using TestTutorial.Models;

namespace TestTutorial.ViewModels
{
	
	public class CollectionViewModel : BaseViewModel
	{	
	    private Cell _current;
	    public Cell Current
	    {
	        get { return _current; }
	        set { _current = value; RaisePropertyChanged(() => Current); }
	    }
	
	    private ObservableCollection<Cell> _list = new ObservableCollection<Cell>();
	
	    public ObservableCollection<Cell> List
	    {
	        get { return _list; }
	        set
	        {
	            _list = value;
	            RaisePropertyChanged(() => List);
	        }
	    }
	
	    public void Init()
	    {
	        for (int i = 1; i <= 3; i++)
        	{
            	Cell cell = new Cell(){ CellValue = i, CellTextureName = "Resources/Icons/Icon_" + i };
				//Cell cell = new Cell(){ CellValue = i, CellTextureName = "StreamingAssets/Icons/Icon_" + i + ".png" };
				
            	List.Add(cell);
			}
        }
    
	   	public ICommand SelectionChangedCommand
	    {
	        get
	        {
	            return new MvxCommand<Cell>(item =>
	            {
	                UnityEngine.Debug.Log((item as Cell).CellValue);
	            });
	        }
	    }
		
	  	public ICommand AddCommand
	    {
	        get
	        {
	            return new MvxCommand(() =>
	            {
	                //UnityEngine.Debug.Log("AddCommand");
					List.Add( new Cell(){ CellValue = 999 } );
	            });
	        }
	    }
		
		public ICommand RemoveCommand
	    {
	        get
	        {
	            return new MvxCommand(() =>
	            {
	                //UnityEngine.Debug.Log("RemoveCommand");
					List.RemoveAt(1);
	
	         		   });
	        }
	    }
		
		public ICommand ReplaceCommand
	    {
	        get
	        {
	            return new MvxCommand(() =>
	            {
	                //UnityEngine.Debug.Log("ReplaceCommand");
	
	            });
	        }
	    }
	
		public ICommand MoveCommand
	    {
	        get
	        {
	            return new MvxCommand(() =>
	            {
	                //UnityEngine.Debug.Log("MoveCommand");
	
	            });
	        }
	    }
		
		public ICommand ReloadCommand
	    {
	        get
	        {
	            return new MvxCommand(() =>
	            {
	                //UnityEngine.Debug.Log("ReloadCommand");
					
					Collection<Cell> list = new Collection<Cell>();
					
					for (int i = 1; i <= 10; i++)
		        	{
						Cell cell = new Cell(){ CellValue = i, CellTextureName = "Resources/Icons/Icon_" + i };
						//Cell cell = new Cell(){ CellValue = i, CellTextureName = "StreamingAssets/Icons/Icon_" + i + ".png" };
		            	
		            	list.Add(cell);
					}
					
					List = new ObservableCollection<Cell>(list);
	
	            });
	        }
	    }

}
}
