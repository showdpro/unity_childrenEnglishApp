#import <UIKit/UIKit.h>
#import "GDTMobInterstitial.h"
#import "GDTMobBannerView.h"
//---------------------------插屏---------------------------
@interface InterstitialViewController : UIViewController<GDTMobInterstitialDelegate>{
@private
    GDTMobInterstitial *_interstitialObj;
    BOOL _isInterstitialReady;
}

-(void)initAd:(NSString*)appkey posID:(NSString*)posID;
-(void)showAd;
-(void)loadAd;
-(BOOL)isInterstitialReady;

@end
//---------------------------插屏end------------------------

@interface UniGdtAdmob:NSObject{
    
	@public
		
	@private
	InterstitialViewController* _interstitialViewController;
}

+(UniGdtAdmob*) getInstance;

-(void)initInterstitial:(NSString*)appkey interstitialID:(NSString*)interstitialID;
-(void)loadInterstitial;
-(void)showInterstitial;
-(BOOL)isInterstitialReady;
	
@end



