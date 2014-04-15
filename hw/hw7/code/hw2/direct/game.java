import java.util.Random;

public class game{
    public static Duck d;
    public static Random ran = new Random();
    public static void changeType(int type){
        switch(type){
            case 1:
                d = new DecoyDuck(true,1);
                break;
            case 3:
                d = new RedheadDuck(true,1);
                break;
            case 2:
                d = new MallardDuck(true,1);
                break;
            case 4:
                d = new RubberDuck(true,1);
                break;
            default:
                d = new Duck(true,1);
                break;

        }
    }
    public static void main(String[] args){
        changeType(ran.nextInt(5));
        int times = 10;
        for(int i=0;i<times;i++){
            System.out.println("========iteration: " + (i+1) + " ==========");
            d.doAll();
            if(i % 2 == 0 && ran.nextInt(2)==0)
                d.modifyFly(!d.canFly);
            if(i % 2 == 0)
                d.modifyQuack(ran.nextInt(3)+1);
        }
    }
}
