import React,{useState, useEffect} from 'react';
import './Test.css';

const mystyle = {
    textAlign: 'center' as 'center',
    height: '500px'
};

const mystyle2 = {
    backgroundColor: 'rgba(255, 255, 255, 0.85)',
    textAlign: 'center' as 'center',
    height: '200px'
};

interface myState {
    scroller: number;
}



const TestList: React.FC = () => {

    const [myState,setMyState] = useState<myState>({scroller: 0});

    useEffect(()=>{
        const handleScroll = () =>{
            setMyState({scroller: document.documentElement.scrollTop});
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) 
            {
                const navbar : HTMLElement | null =  document.getElementById("navbar");
                if(navbar)
                {
                    navbar.style.top = "0";
                }
            } else {
                const navbar : HTMLElement | null =  document.getElementById("navbar");
                if(navbar)
                {
                    navbar.style.top = "-50px";
                }
            }
        }
        window.addEventListener("scroll", handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    },[myState])

    return (
        <div>
            <div id="navbar">
                <a href="#home">Home</a>
                <a href="#news">News</a>
                <a href="#contact">Contact</a>
            </div>
            <div className="parallax-image img1">
                <div style={mystyle}></div>
            </div>
            <div className="parallax-image img1">
                <div style={mystyle2}></div>
            </div>
            <div className="parallax-image img2">
                <div className="masonry">
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/06/29/70/062970b4a7f0ab403c0e693e7ec8cea6.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/66/fc/f8/66fcf805c084cda7a56ac7e7cceca359.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/8c/0f/03/8c0f03a51af26cd9ff050d937ac9dc2b.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/61/65/08/616508193c671f7525b8308e32664901.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/c1/33/54/c1335491e362fe6f0fa2fb7c2c991341.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/54/f7/e1/54f7e120c9b779c8ff0d2b34da93f854.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/8c/4e/15/8c4e15ae93f206459d8c5f7c8a032252.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                    <div className="item">
                        <img src='https://i.pinimg.com/236x/02/74/08/02740885f3a909dfca1afa218191dc8b.jpg'/>
                        <div className="overlay">
                            <div className="text">Hello World</div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="parallax-image img3">
                <div style={mystyle2}></div>
            </div>
        </div>
    );
}

export default TestList;