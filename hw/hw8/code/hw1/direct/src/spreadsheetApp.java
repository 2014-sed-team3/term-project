	import javafx.application.Application;
	import javafx.beans.property.SimpleStringProperty;
	import javafx.collections.FXCollections;
	import javafx.collections.ObservableList;
import javafx.event.EventHandler;
	import javafx.geometry.Insets;
	import javafx.scene.Group;
	import javafx.scene.Scene;
	import javafx.scene.control.Label;
	import javafx.scene.control.TableColumn;
import javafx.scene.control.TableColumn.CellEditEvent;
	import javafx.scene.control.TableView;
	import javafx.scene.control.TextField;
	import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.control.cell.TextFieldTableCell;
	import javafx.scene.layout.VBox;
	import javafx.scene.text.Font;
import javafx.scene.text.Text;
import javafx.stage.Stage;

public class spreadsheetApp{
    
	private static spreadsheetData mData;
	private static Table mTable;
	private static BarChart mBarChart;
	private static PieChart mPieChart;
	   
	public static void main(String[] args) {
	        init();
	        double[][] d=new double[4][4];
	        for (int i=0;i<4;i++){
	        	for(int j=0;j<4;j++){
	        		d[i][j]=i*j;
	        	}
	        }
	        mData.setCharts(mTable, mBarChart, mPieChart);
	        mData.setData(d);
	        mTable.onUpdate();
	}
	public static void init(){
		mData=new spreadsheetData();
		mTable=new Table();
		mBarChart=new BarChart();
		mPieChart=new PieChart();
	}

	} 
    
