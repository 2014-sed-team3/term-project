
public class spreadsheetData {
	public double[][] mData;
	//public String[] mColName;
	//public String[] mRowName;
	private Table mTable;
	private BarChart mBarChart;
	private PieChart mPieChart;
	public void setCharts(Table t,BarChart b,PieChart p){
		mTable=t;
		mBarChart=b;
		mPieChart=p;
	}
	public void setData(double[][] newData){
		mData=newData;
		onUpdate();
	}
	public void onUpdate(){
		mTable.show(this);
		mBarChart.show(this);
		mPieChart.show(this);
	}
}
