
public class SpreadsheetData {
	private double[][] m_information;
	private Spreadsheet m_sp;
	private BarChart m_bar;
	private PieChart m_pie;
	
	public void update(double[][] info){
		m_information=info;
		m_sp.depict("", info);
		m_bar.depict("", info);
		m_pie.depict("", info);
	}
}
