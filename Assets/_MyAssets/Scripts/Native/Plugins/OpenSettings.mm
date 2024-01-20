@implementation OpenSettings
 
extern "C" {
    void Open();
}
 
void Open()
{
     NSURL *url = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
     [[UIApplication sharedApplication] openURL:url];
}
 
@end