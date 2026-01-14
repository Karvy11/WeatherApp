#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern "C" {
    void _ShowNativeAlert(const char* message) {
        NSString *msgString = [NSString stringWithUTF8String:message];
        
        UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"Weather App"
                                                                       message:msgString
                                                                preferredStyle:UIAlertControllerStyleAlert];
        
        UIAlertAction *okAction = [UIAlertAction actionWithTitle:@"OK"
                                                           style:UIAlertActionStyleDefault
                                                         handler:nil];
        
        [alert addAction:okAction];
        
        UIViewController *vc = UnityGetGLViewController();
        [vc presentViewController:alert animated:YES completion:nil];
    }
}