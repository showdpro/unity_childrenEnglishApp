#import "UniGdtAdmob.h"
//---------------------------C/C++------------------------
extern UIViewController *UnityGetGLViewController();
extern "C" void UnitySendMessage(const char *, const char *, const char *);
NSString* UniGdtMakeNSString (const char* string) {
    if (string) {
        return [NSString stringWithUTF8String: string];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

char* UniGdtMakeCString(NSString *str) {
    const char* string = [str UTF8String];
    if (string == NULL) {
        return NULL;
    }
	
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"{
	typedef void (*UniGdtAdCallback)(const char*adtype, const char* eventName, const char*msg);
	UniGdtAdCallback _uniGdtCallback;
	
	void _UniGdtInitInterstitial(const char* appkey, const char*interstitialID,UniGdtAdCallback gdtCallback){
		[[UniGdtAdmob getInstance] initInterstitial:UniGdtMakeNSString(appkey) interstitialID:UniGdtMakeNSString(interstitialID)];
		_uniGdtCallback=gdtCallback;
	}
	void _UniGdtLoadInterstitial(){
		[[UniGdtAdmob getInstance] loadInterstitial];
	}
	void _UniGdtShowInterstitial(){
		[[UniGdtAdmob getInstance] showInterstitial];
	}
	bool _UniGdtIsInterstitialReady(){
		return [[UniGdtAdmob getInstance] isInterstitialReady];
	}
	
}
//---------------------------C/C++ end--------------------

//---------------------------插屏-------------------------
#define IS_OS_7_OR_LATER    ([[[UIDevice currentDevice] systemVersion] floatValue] >= 7.0)
@implementation InterstitialViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad{
    if (IS_OS_7_OR_LATER) {
        self.extendedLayoutIncludesOpaqueBars = NO;
        self.edgesForExtendedLayout = UIRectEdgeBottom | UIRectEdgeLeft | UIRectEdgeRight;
    }
    [super viewDidLoad];
    //
}

-(void)initAd:(NSString*)appkey posID:(NSString*)posID{
	_interstitialObj = [[GDTMobInterstitial alloc] initWithAppkey:appkey placementId:posID];
	_interstitialObj.delegate = self; //设置委托
	_interstitialObj.isGpsOn = NO; //【可选】设置GPS开关
	[self loadAd];//初始化就加载
}
//显示广告
-(void)showAd{
    [_interstitialObj presentFromRootViewController:UnityGetGLViewController()];
    
}
//预加载
-(void)loadAd{
	_isInterstitialReady=NO;
	[_interstitialObj loadAd];
}
//插屏广告是否准备好了
-(BOOL)isInterstitialReady{
    NSLog(@"===gdt _isInterstitialReady:%i",_isInterstitialReady);
	return _isInterstitialReady;
}

//广告预加载成功回调
// 详解:当接收服务器返回的广告数据成功后调用该函数
-(void)interstitialSuccessToLoadAd:(GDTMobInterstitial *)interstitial{
	_isInterstitialReady=YES;
    NSLog(@"===gdt interstitialSuccessToLoadAd");
	_uniGdtCallback(UniGdtMakeCString(@"interstitial"),UniGdtMakeCString(@"onAdLoadedGdt"),UniGdtMakeCString(@""));
}

// 广告预加载失败回调
// 详解:当接收服务器返回的广告数据失败后调用该函数
- (void)interstitialFailToLoadAd:(GDTMobInterstitial *)interstitial error:(NSError *)error{
    NSLog(@"===gdt interstitialFailToLoadAd");
    _uniGdtCallback(UniGdtMakeCString(@"interstitial"),UniGdtMakeCString(@"onAdFailedToLoadGdt"),UniGdtMakeCString(@""));
}

// 插屏广告将要展示回调
// 详解: 插屏广告即将展示回调该函数
- (void)interstitialWillPresentScreen:(GDTMobInterstitial *)interstitial{
    
}

// 插屏广告视图展示成功回调
// 详解: 插屏广告展示成功回调该函数
- (void)interstitialDidPresentScreen:(GDTMobInterstitial *)interstitial{
   
}

// 插屏广告展示结束回调
// 详解: 插屏广告展示结束回调该函数
- (void)interstitialDidDismissScreen:(GDTMobInterstitial *)interstitial{
    NSLog(@"===onAdClosedGdt");
    _uniGdtCallback(UniGdtMakeCString(@"interstitial"),UniGdtMakeCString(@"onAdClosedGdt"),UniGdtMakeCString(@""));
}

// 应用进入后台时回调
//
// 详解: 当点击下载应用时会调用系统程序打开，应用切换到后台
- (void)interstitialApplicationWillEnterBackground:(GDTMobInterstitial *)interstitial{
    
}
//---------------------------插屏end------------------------
@end


//---------------------------UniGdtAdmob------------------
@implementation UniGdtAdmob
+(UniGdtAdmob*)getInstance{
    static dispatch_once_t once;
    static UniGdtAdmob *instance;
    dispatch_once(&once, ^ { instance = [[UniGdtAdmob alloc] init]; });
    return instance;
}

-(void)initInterstitial:(NSString*)appkey interstitialID:(NSString*)interstitialID{
    _interstitialViewController=[[InterstitialViewController alloc] initWithNibName:@"GdtAdInterstitialViewController" bundle:nil];
    [UnityGetGLViewController().navigationController pushViewController:_interstitialViewController animated:YES];
    //[UnityGetGLViewController() addChildViewController:_interstitialViewController];
    
    [_interstitialViewController initAd:appkey posID:interstitialID];
}
-(void)loadInterstitial{
    [_interstitialViewController loadAd];
}
-(void)showInterstitial{
    [_interstitialViewController showAd];
}
-(BOOL)isInterstitialReady{
    return [_interstitialViewController isInterstitialReady];
}
//---------------------------UniGdtAdmob end------------------
@end

