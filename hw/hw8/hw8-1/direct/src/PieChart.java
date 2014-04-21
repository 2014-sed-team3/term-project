
public class PieChart {
	private spreadsheetData mData;
	
	public void show(spreadsheetData newData){
		mData=newData;
		System.out.format("pie\n%s\n",mData.mData.toString());
		System.out.format("table\n%f\n",mData.mData[0][0]);
		//show newData on screen
	}
	public void onUpdate(){
		//modified mData
		mData.onUpdate();
	}

}
