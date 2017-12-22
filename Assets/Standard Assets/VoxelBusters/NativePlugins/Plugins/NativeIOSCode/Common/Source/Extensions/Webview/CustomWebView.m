//
//  CustomWebView.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 19/12/14.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "CustomWebView.h"

@implementation CustomWebView

#define cToolBarHeight  44

@synthesize isShowing = _isShowing;
@synthesize canDismiss;
@synthesize canBounce;
@synthesize controlType;
@synthesize showSpinnerOnLoad;
@synthesize autoShowOnLoadFinish;
@synthesize scalesPageToFit;
@synthesize allowMediaPlayback;
@synthesize normalisedFrame;

@synthesize closeButton;
@synthesize loadingSpinner;
@synthesize webview;
@synthesize toolbar;
@synthesize webviewTag;
@synthesize URLSchemeList;

+ (id)CreateWithFrame:(CGRect)frame tag:(NSString *)tag
{
	return [[[self alloc] initWithFrame:frame tag:tag] autorelease];
}

- (id)initWithFrame:(CGRect)frame
{
	return [self initWithFrame:frame tag:@"no-tag"];
}

- (id)initWithFrame:(CGRect)frame tag:(NSString *)tag
{
    self = [super initWithFrame:frame];
    
    if (self)
    {
		// Tag
		self.webviewTag					= tag;
		
		// Create webview
		[self createWebView];
		
		// Create toolbar
		[self createToolbar];
		
		// Create close button
		[self createCloseButton];
		
        // Create activity indicator
        [self createLoadingSpinner];
		
		// Defaults
		_isShowing						= NO;
		self.URLSchemeList  			= [NSMutableArray array];
		self.canDismiss					= YES;
		self.canBounce					= YES;
		self.controlType				= WebviewControlTypeCloseButton;
		self.showSpinnerOnLoad			= YES;
		self.autoShowOnLoadFinish		= YES;
		self.scalesPageToFit			= YES;
		self.allowMediaPlayback			= YES;
		
		// Set color
		[self setBackgroundColor:[UIColor whiteColor]];
		
		// Add as observer
		[[UIDeviceOrientationManager Instance] setObserver:self];
    }
    
    return self;
}

- (void)dealloc
{
	// Remove observer
	[[UIDeviceOrientationManager Instance] removeObserver:self];
	[self.webview setDelegate:nil];
	[self.webview stopLoading];
	
	// Release
	self.closeButton	= NULL;
    self.loadingSpinner	= NULL;
	self.webview		= NULL;
    self.toolbar     	= NULL;
    self.URLSchemeList	= NULL;
    self.webviewTag     = NULL;
    
    [super dealloc];
}

- (void)createWebView
{
	self.webview	= [[[UIWebView alloc] init] autorelease];
	
	// Set delegate
	[self.webview setDelegate:self];
	[self.webview setOpaque:NO];
	[self.webview setMediaPlaybackRequiresUserAction:NO];
	[self.webview setAllowsInlineMediaPlayback:YES];
	
	// Add to view
	[self addSubview:self.webview];
}

- (void)createToolbar
{
	self.toolbar            = [[[WebViewToolBar alloc] init] autorelease];
	
	// Set delegate
	[self.toolbar setToolbarDelegate:self];
	
	// Add to view
	[self addSubview:self.toolbar];
}

- (void)createCloseButton
{
	UIImage *closeBtnImage	= [UIImage imageNamed:@"close_button.png"];
	self.closeButton		= [UIButton buttonWithType:UIButtonTypeCustom];
	
	// Set action and default image
	[self.closeButton setImage:closeBtnImage
					  forState:UIControlStateNormal];
	[self.closeButton addTarget:self
						 action:@selector(onPressingCloseButton:)
			   forControlEvents:UIControlEventTouchUpInside];
	[self.closeButton setFrame:CGRectMake(0, 						0,
										  closeBtnImage.size.width, closeBtnImage.size.height)];
	
	// Add to view
	[self addSubview:self.closeButton];
}

- (void)createLoadingSpinner
{
	self.loadingSpinner  	= [[[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray] autorelease];
	
	// Set properties
	[self.loadingSpinner setHidesWhenStopped:YES];
	
	// Add to view
	[self addSubview:self.loadingSpinner];
}

#pragma mark - Properties

- (void)setCanDismiss:(BOOL)dismiss
{
	canDismiss		= dismiss;
	
	// Update button state
	[[self closeButton] setEnabled:dismiss];
	[[self toolbar] setCanStop:dismiss];
}

- (void)setCanBounce:(BOOL)bounces
{
	canBounce		= bounces;
	
	// Update webview property
	[[[self webview] scrollView] setBounces:bounces];
}

- (void)setControlType:(WebviewControlType)newType
{
    controlType      	= newType;
	
	// By default toolbar and close button are hidden
	[[self toolbar] setHidden:YES];
	[[self closeButton] setHidden:YES];
	
    if (newType == WebviewControlTypeToolbar)
	{
		[[self toolbar] setHidden:NO];
	}
	else if (newType == WebviewControlTypeCloseButton)
	{
		[[self closeButton] setHidden:NO];
	}
	
	// Update frame
	[self updateWebViewFrame];
}

- (void)setShowSpinnerOnLoad:(BOOL)show
{
    showSpinnerOnLoad = show;
    
    // Show activity indicator if webview is loading
    if ([[self webview] isLoading] && showSpinnerOnLoad)
	{
		[self showLoadingSpinner];
	}
	else
	{
		[self hideLoadingSpinner];
	}
}

- (void)setScalesPageToFit:(BOOL)scales
{
	scalesPageToFit		= scales;
	
	// Update webview property
	[[self webview] setScalesPageToFit:scales];
}

- (void)setBackgroundColor:(UIColor *)backgroundColor
{
	[super setBackgroundColor:[UIColor clearColor]];
	
	// Apply same property to webview
	[[self webview] setBackgroundColor:backgroundColor];
}

- (void)setAllowMediaPlayback:(BOOL)allow
{
	allowMediaPlayback	= allow;
	
	// Update webview property
	[[self webview] setAllowsInlineMediaPlayback:allow];
}

#pragma mark - View

- (void)setFrame:(CGRect)frame
{
	// Update normalised rect
	[self setNormalisedFrame:ConvertToNormalisedRect(frame)];
}

- (void)setNormalisedFrame:(CGRect)newFrame
{
	normalisedFrame	= newFrame;

	// Update frame
	[self updateWebViewFrame];
}

- (void)updateWebViewFrame
{
	// First set this views frame
	[super setFrame:ConvertToApplicationSpace([self normalisedFrame])];
	
	// Now update internal subview size
	CGRect viewFrame		= [self frame];
	CGSize viewSize			= viewFrame.size;
	CGRect webviewFrame;

	// Based on control style frame is set
	if ([self controlType] == WebviewControlTypeToolbar)
	{
		// Set webview origin and size
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size   = CGSizeMake(viewSize.width, viewSize.height - cToolBarHeight);
	}
	else if ([self controlType] == WebviewControlTypeCloseButton)
	{
		CGSize closeButtonSize;
		
		if (self.closeButton != NULL)
		{
			CALayer* closeBtnLayer	= [[self closeButton] layer];
			
			// Cache button size
			closeButtonSize	= [[self closeButton] frame].size;
			
			// Update anchor and position
			[closeBtnLayer setAnchorPoint:CGPointMake(1, 0)];
			[closeBtnLayer setPosition:CGPointMake(CGRectGetMaxX(viewFrame), 0)];
		}
		
		// Set webview origin and size
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size	= viewSize;
	}
	else
	{
		// Set webview origin and size
		webviewFrame.origin	= CGPointMake(0, 0);
		webviewFrame.size	= viewSize;
	}
	
	// Set webview frame
	[[self webview] setFrame:webviewFrame];
	
	// Update toolbar position
	if (self.toolbar)
	{
		[[self toolbar] setFrame:CGRectMake(0, 				CGRectGetMaxY(webviewFrame),
											viewSize.width,	cToolBarHeight)];
	}
	
	// Set anchor to (0.5, 0.5) and update spinner position
	if (self.loadingSpinner)
	{
		[[[self loadingSpinner] layer] setAnchorPoint:CGPointMake(0.5, 0.5)];
		[[[self loadingSpinner] layer] setPosition:CGPointMake(CGRectGetMidX(webviewFrame), CGRectGetMidY(webviewFrame))];
	}
}

- (void)show
{
    NSLog(@"[CustomWebView] show %@", [self webviewTag]);
	
	// Update property
	_isShowing	= YES;
}

- (void)dismiss
{
	NSLog(@"[CustomWebView] dismiss %@", [self webviewTag]);
	
	// Update property
	_isShowing	= NO;
	
	// Stop request
	[self stopLoading];
	
	// Remove view
	[self removeFromSuperview];
}

- (void)layoutSubviews
{
	// Update web frame
	[self updateWebViewFrame];
}

#pragma mark - Load

- (void)loadRequest:(NSURLRequest *)request
{
	[[self webview] loadRequest:request];
}

- (void)loadHTMLString:(NSString *)string baseURL:(NSURL *)baseURL
{
	[[self webview] loadHTMLString:string
						   baseURL:baseURL];
}

- (void)loadData:(NSData *)data MIMEType:(NSString *)MIMEType textEncodingName:(NSString *)textEncodingName baseURL:(NSURL *)baseURL
{
	[[self webview] loadData:data
					MIMEType:MIMEType
			textEncodingName:textEncodingName
					 baseURL:baseURL];
}

- (NSString *)stringByEvaluatingJavaScriptFromString:(NSString *)script
{
	NSString* result    = [[self webview] stringByEvaluatingJavaScriptFromString:script];
	
    return result;
}

- (void)reload
{
	[[self webview] reload];
}

- (void)stopLoading
{
	// Stops loading webview
	[[self webview] stopLoading];
	
	// Hide activity spinner views
	[self hideLoadingSpinner];
	[[UIApplication sharedApplication] setNetworkActivityIndicatorVisible:NO];
}

#pragma mark - URL Scheme

- (void)addNewURLScheme:(NSString *)scheme
{
    [self.URLSchemeList addObject:scheme];
}

- (BOOL)shouldStartLoadRequestWithURLScheme:(NSString *)URLScheme
{
	return NO;
}

- (void)foundMatchingURLScheme:(NSURL *)requestURL
{
    NSLog(@"[CustomWebView] found matching URL scheme: %@", [requestURL scheme]);
}

#define kHost		@"host"
#define kArguments	@"arguments"
#define kURLScheme	@"url-scheme"
#define kURL		@"url"

- (NSMutableDictionary *)parseURLScheme:(NSURL *)requestURL
{
	NSString *scheme  	= [requestURL scheme];
	NSString *query		= [requestURL query];
	NSString *host		= [requestURL host];
	
	// Get arguments
	NSMutableDictionary *argsDict	= [NSMutableDictionary dictionary];
	NSArray *queryParts				= [query componentsSeparatedByString:@"&"] ;
	
	if ([queryParts count] > 0)
	{
		for (NSString *keyValuePair in queryParts)
		{
			NSArray *kvParts	= [keyValuePair componentsSeparatedByString:@"="];
			
			if ([kvParts count] == 2)
			{
				NSString *key		= [kvParts objectAtIndex:0];
				NSString *value		= [kvParts objectAtIndex:1];
				argsDict[key]		= value;
			}
		}
	}
	
	// Allot scheme data dictionary
	NSMutableDictionary *messageDict	= [NSMutableDictionary dictionary];
	messageDict[kURL]					= [requestURL absoluteString];
	messageDict[kURLScheme]				= scheme;
	messageDict[kHost]					= IsNullOrEmpty(host) ? kNSStringDefault : host;
	messageDict[kArguments] 			= argsDict;
	
	return messageDict;
}

#pragma mark - Toolbar Delegate

- (void)onPressingBack
{
    NSLog(@"[CustomWebView] user pressed go back");
   
	// Go back
	[[self webview] goBack];
}

- (void)onPressingStop
{
    NSLog(@"[CustomWebView] user pressed done");
	
	// Remove from view
	[self dismiss];
}

- (void)onPressingReload
{
    NSLog(@"[CustomWebView] user pressed reload");
    
	// Reload
	[[self webview] reload];
}

- (void)onPressingForward
{
    NSLog(@"[CustomWebView] user pressed go forward");
   
	// Go forward
	[[self webview] goForward];
}

#pragma mark - Button Callback

- (void)onPressingCloseButton:(id)sender
{
	[self onPressingStop];
}

#pragma mark - Misc.

- (void)showLoadingSpinner
{
	[self.loadingSpinner setHidden:NO];
	[self.loadingSpinner startAnimating];
}

- (void)hideLoadingSpinner
{
	[self.loadingSpinner setHidden:YES];
	[self.loadingSpinner stopAnimating];
}

- (void)onRequestStateChange:(BOOL)isLoading
{
    if (isLoading)
    {
        // Show network activity indicator
        [UIApplication sharedApplication].networkActivityIndicatorVisible   = YES;
		
		// By default we will hide loading spinner
		[self hideLoadingSpinner];
		
		// Show spinner if required
		if (self.showSpinnerOnLoad)
		{
			[self showLoadingSpinner];
		}
    }
    else
    {
        // Hide network activity indicator
        [UIApplication sharedApplication].networkActivityIndicatorVisible   = NO;
        
        // Hide loading spinner
		[self hideLoadingSpinner];
	}
	
	// Update state of toolbar buttons
	[[self toolbar] setCanGoBack:self.webview.canGoBack];
	[[self toolbar] setCanGoForward:self.webview.canGoForward];
}

#pragma mark - Webview Delegate

- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType
{
    NSURL *requestURL            		= [request URL];
    NSString *currentURLScheme          = [requestURL scheme];
    
    for (NSString *scheme in self.URLSchemeList)
    {
         if ([currentURLScheme caseInsensitiveCompare:scheme] == NSOrderedSame)
         {
			 // Whenever a new matching URL scheme is found, notify
			 [self foundMatchingURLScheme:requestURL];
			 
			 // Check if we need to load this URL
			 return [self shouldStartLoadRequestWithURLScheme:scheme];
         }
    }
    
    return YES;
}

- (void)webViewDidStartLoad:(UIWebView *)webView
{
    NSLog(@"[CustomWebView] did start loading, tag %@", [self webviewTag]);
    
    // Loading started
    [self onRequestStateChange:TRUE];
}

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
    NSLog(@"[CustomWebView] did finish loading, tag %@", [self webviewTag]);
    
    // Done with loading
    [self onRequestStateChange:FALSE];
    
    // Show webview, if auto show is enabled
    if ([self autoShowOnLoadFinish])
        [self show];
}

- (void)webView:(UIWebView *)webView didFailLoadWithError:(NSError *)error
{
    NSLog(@"[CustomWebView] did fail loading, tag %@", [self webviewTag]);
    
    // Done with loading
    [self onRequestStateChange:FALSE];
}

#pragma mark - Orientation Observer

- (void)didRotateToOrientation:(UIDeviceOrientation)toOrientation fromOrientation:(UIDeviceOrientation)fromOrientation
{
	[self setNeedsLayout];
}

@end