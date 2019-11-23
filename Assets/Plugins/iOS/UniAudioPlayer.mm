#import <CommonCrypto/CommonDigest.h>
#import "AudioStreamPlayer/HSUAudioStreamPlayer.h"
#import "UniAudioPlayer.h"

#define DOC_FILE(s) [[NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES) objectAtIndex:0] stringByAppendingPathComponent:s]
NSString *md5Hash (NSString *str);

NSString* UniAudioPlayerMakeNSString (const char* string) {
    if (string) {
        return [NSString stringWithUTF8String: string];
    } else {
        return [NSString stringWithUTF8String: ""];
    }
}

char* UniAudioPlayerMakeCString(NSString *str) {
    const char* string = [str UTF8String];
    if (string == NULL) {
        return NULL;
    }
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"{
    typedef void (*UniAudioStateChangedCallback)(const char*state);
    UniAudioStateChangedCallback _uniAudioStateChangedCallback;
    void _UniAudioPlayerInitialize(UniAudioStateChangedCallback uniAudioStateChangedCallback);
    void _UniAudioPlayerPlayUrl(char* urlString);
    void _UniAudioPlayerPlay();
    void _UniAudioPlayerPause();
    void _UniAudioPlayerStop();
    void _UniAudioPlayerSeekToTime(double time);
    void _UniAudioPlayerSetIsLoop(BOOL value);
    bool _UniAudioPlayerIsLoop();
    double _UniAudioPlayerGetCurrentTime();
    double _UniAudioPlayerGetProgress();
    double _UniAudioPlayerGetDuration();
    float _UniAudioPlayerGetCurrentVolume();
    BOOL _UniAudioPlayerIsPlaying();
}

void _UniAudioPlayerInitialize(UniAudioStateChangedCallback uniAudioStateChangedCallback){
    _uniAudioStateChangedCallback=uniAudioStateChangedCallback;
}

void _UniAudioPlayerPlayUrl(char* urlString){
    [[UniAudioPlayer getInstance] playUrl:UniAudioPlayerMakeNSString(urlString)];
}

void _UniAudioPlayerPlay(){
    [[UniAudioPlayer getInstance] play];
}

void _UniAudioPlayerPause(){
    [[UniAudioPlayer getInstance] pause];
}

void _UniAudioPlayerStop(){
    [[UniAudioPlayer getInstance] stop];
}

void _UniAudioPlayerSeekToTime(double time){
    [[UniAudioPlayer getInstance] seekToTime:time];
}

void _UniAudioPlayerSetIsLoop(BOOL value){
    [UniAudioPlayer getInstance]->isLoop=value;
}

bool _UniAudioPlayerIsLoop(){
    return [UniAudioPlayer getInstance]->isLoop;
}

double _UniAudioPlayerGetCurrentTime(){
    return [[UniAudioPlayer getInstance] currentTime];
}

double _UniAudioPlayerGetProgress(){
    return [[UniAudioPlayer getInstance] progress];
}

double _UniAudioPlayerGetDuration(){
    return [[UniAudioPlayer getInstance] duration];
}

float _UniAudioPlayerGetCurrentVolume(){
    return [[UniAudioPlayer getInstance] currentVolume];
}

BOOL _UniAudioPlayerIsPlaying(){
    return  [[UniAudioPlayer getInstance] isPlaying];
}


@interface UniAudioPlayer()

@end

@implementation UniAudioPlayer{
		HSUAudioStreamPlayer* player;
}



+ (UniAudioPlayer *) getInstance {
    static dispatch_once_t once;
    static UniAudioPlayer *instance;
    dispatch_once(&once, ^ { instance = [[UniAudioPlayer alloc] init]; });
    return instance;
}

-(void)playUrl:(NSString*)urlString{
		
    if(player){
    	[player stop];
    	player=nil;
    }
    
    HSUAudioStreamPlayBackState state = player.state;
    if (!player || state == HSU_AS_STOPPED) {
    		_curUrl=urlString;
        NSString *urlHash = [NSString stringWithFormat:@"%@.mp3", md5Hash(urlString)];
        NSString *cacheFile = DOC_FILE(urlHash);
        player = [[HSUAudioStreamPlayer alloc]
                       initWithURL:[NSURL URLWithString:urlString]
                       cacheFilePath:cacheFile];
        [[NSNotificationCenter defaultCenter]
         addObserver:self
         selector:@selector(playBackStateChanged:)
         name:HSUAudioStreamPlayerStateChangedNotification
         object:player];
        [[NSNotificationCenter defaultCenter]
         addObserver:self
         selector:@selector(playBackDurationChanged:)
         name:HSUAudioStreamPlayerDurationUpdatedNotification
         object:player];
        [player play];
    }
}

-(void)playBackDurationChanged:(NSNotification *)notification{
    NSString* duration = [NSString stringWithFormat:@"%gs", ceil(player.duration)];
    
}

-(void)playBackStateChanged:(NSNotification *)notification{
	HSUAudioStreamPlayer *player = notification.object;
    HSUAudioStreamPlayBackState state = player.state;
    if (state == HSU_AS_PLAYING) {
        _uniAudioStateChangedCallback(UniAudioPlayerMakeCString(@"HSU_AS_PLAYING"));
    }else if (state == HSU_AS_PAUSED) {
       _uniAudioStateChangedCallback(UniAudioPlayerMakeCString(@"HSU_AS_PAUSED"));
    }else if (state == HSU_AS_WAITTING) {
      _uniAudioStateChangedCallback(UniAudioPlayerMakeCString(@"HSU_AS_WAITTING"));
    }else if (state == HSU_AS_STOPPED) {
    	_uniAudioStateChangedCallback(UniAudioPlayerMakeCString(@"HSU_AS_STOPPED"));
    }else if (state == HSU_AS_FINISHED) {
    	if(isLoop){
			[self playUrl:_curUrl];
		}
    	_uniAudioStateChangedCallback(UniAudioPlayerMakeCString(@"HSU_AS_FINISHED"));
    }
}

-(void)play{
	[player play];
}

-(void)pause{
		[player pause];
}

-(void)stop{
		[player stop];
}

-(void)seekToTime:(double)time{
    [player seekToTime:time];
}

-(double)currentTime{
		return [player currentTime];
}

-(double)progress{
		return [player progress];
}

-(double)duration{
		return [player duration];
}

-(float)currentVolume{
		return [player currentVolume];
}
-(BOOL)isPlaying{
	return player.state==HSU_AS_PLAYING;
}

@end
NSString *md5Hash (NSString *str){
    unsigned char result[16];
    NSData *data = [str dataUsingEncoding:NSUTF8StringEncoding];
    CC_MD5(data.bytes, data.length, result);
    
    return [NSString stringWithFormat:
            @"%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x%02x",
            result[0], result[1], result[2], result[3], result[4], result[5], result[6], result[7],
            result[8], result[9], result[10], result[11], result[12], result[13], result[14], result[15]
            ];
}




