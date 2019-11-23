#import <UIKit/UIKit.h>


@interface UniAudioPlayer:NSObject{
	@public
		BOOL isLoop;
	@private NSString* _curUrl;
}
+(UniAudioPlayer*) getInstance;

-(void)playBackDurationChanged:(NSNotification *)notification;
-(void)playBackStateChanged:(NSNotification *)notification;
-(void)playUrl:(NSString*)urlString;
-(void)play;
-(void)pause;
-(void)stop;
-(void)seekToTime:(double)time;
	
-(double)currentTime;
-(double)progress;
-(double)duration;
-(float)currentVolume;
-(BOOL)isPlaying;
@end
