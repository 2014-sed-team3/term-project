public class BlackBorder implements DisplayComponent{
    private int blackBorderSize = 0; //defalut as not show
    public BlackBorder(int size){
        blackBorderSize = size;
    }
    public BlackBorder(){}
    public void setBlackBorder(int size){
        blackBorderSize = size;
    }
    public void show(){
        if(blackBorderSize > 0){ 
            // show scrollbar
        }
    }
}
