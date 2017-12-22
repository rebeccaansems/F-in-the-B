//
//  CustomWebView.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "WebViewToolBar.h"
#import "UIDeviceOrientationManager.h"

enum WebviewControlType
{
	WebviewControlTypeNone,
	WebviewControlTypeCloseButton,
	WebviewControlTypeToolbar
};
typedef enum WebviewControlType WebviewControlType;

@interface CustomWebView : UIView <WebViewToolBarDelegate, UIWebViewDelegate, UIDeviceOrientationObserver>

// Properties
@property(nonatomic, readonly)  BOOL                        isShowing;
@property(nonatomic)            BOOL                        canDismiss;
@property(nonatomic)            BOOL                        canBounce;
@property(nonatomic)            WebviewControlType			controlType;
@property(nonatomic)            BOOL                        showSpinnerOnLoad;
@property(nonatomic)            BOOL                        autoShowOnLoadFinish;
@property(nonatomic)            BOOL                        scalesPageToFit;
@property(nonatomic)            BOOL                        allowMediaPlayback;
@property(nonatomic) 			CGRect						normalisedFrame;

@property(nonatomic, retain)    UIButton					*closeButton;
@property(nonatomic, retain)    UIActivityIndicatorView     *loadingSpinner;
@property(nonatomic, retain)    UIWebView					*webview;
@property(nonatomic, retain)    WebViewToolBar              *toolbar;
@property(nonatomic, retain)    NSString                    *webviewTag;
@property(nonatomic, retain)    NSMutableArray              *URLSchemeList;

// Class method
+ (id)CreateWithFrame:(CGRect)frame tag:(NSString *)tag;

// Initialise
- (id)initWithFrame:(CGRect)frame tag:(NSString *)tag;

// Related to view
- (void)show;
- (void)dismiss;

// Loading
- (void)loadRequest:(NSURLRequest *)request;
- (void)loadHTMLString:(NSString *)string baseURL:(NSURL *)baseURL;
- (void)loadData:(NSData *)data
		MIMEType:(NSString *)MIMEType
textEncodingName:(NSString *)textEncodingName
		 baseURL:(NSURL *)baseURL;
- (NSString *)stringByEvaluatingJavaScriptFromString:(NSString *)script;
- (void)reload;
- (void)stopLoading;

// URL Scheme
- (void)addNewURLScheme:(NSString *)scheme;
- (BOOL)shouldStartLoadRequestWithURLScheme:(NSString *)URLScheme;
- (void)foundMatchingURLScheme:(NSURL *)requestURL;
- (NSMutableDictionary *)parseURLScheme:(NSURL *)requestURL;

@end
