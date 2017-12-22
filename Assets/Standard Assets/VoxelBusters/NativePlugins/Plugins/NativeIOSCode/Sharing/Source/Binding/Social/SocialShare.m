//
//  SocialShare.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 09/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "SocialShare.h"

@implementation SocialShare

#define kSocialShareFinished 	"SocialShareFinished"

#pragma mark - Methods

- (NSString *)getServiceName:(SocialShareServiceType)serviceType
{
	if (serviceType == SocialShareServiceTypeFacebook)
	{
		return SLServiceTypeFacebook;
	}
	else if (serviceType == SocialShareServiceTypeTwitter)
	{
		return SLServiceTypeTwitter;
	}
	
	return NULL;
}

- (BOOL)isServiceTypeAvailable:(SocialShareServiceType)serviceType;
{
	NSString *serviceName	= [self getServiceName:serviceType];
	
	if (serviceName != NULL)
	{
		return [SLComposeViewController isAvailableForServiceType:serviceName];
	}
	
	return false;
}

- (void)share:(SocialShareServiceType)serviceType
  withMessage:(NSString *)message
	  withURL:(NSString *)URLString
	 andImage:(UIImage *)image
{
	NSString *serviceName	= [self getServiceName:serviceType];
	
	// Check if service is not available
	if (serviceName == NULL || ![SLComposeViewController isAvailableForServiceType:serviceName])
	{
		// Invoke handler
		[self onFinishingSocialShare:serviceType
						  withResult:SLComposeViewControllerResultCancelled];
		return;
	}
	
	// Share
	SLComposeViewController *shareVC = [SLComposeViewController composeViewControllerForServiceType:serviceName];
	
	if (message)
		[shareVC setInitialText:message];
	
	if (URLString)
		[shareVC addURL:[NSURL URLWithString:URLString]];
	
	if (image)
		[shareVC addImage:image];
	
	[shareVC setCompletionHandler:^(SLComposeViewControllerResult result){
		
		// Invoke handler
		[self onFinishingSocialShare:serviceType
						  withResult:result];
	}];
	
	// Present view
	[UnityGetGLViewController() presentViewController:shareVC
											 animated:YES
										   completion:nil];
}

- (void)onFinishingSocialShare:(SocialShareServiceType)serviceType withResult:(SLComposeViewControllerResult)result
{
	// Notify unity
	NSMutableDictionary *dataDict	= [NSMutableDictionary dictionary];
	[dataDict setObject:[NSNumber numberWithInt:result] forKey:@"result"];
	[dataDict setObject:[NSNumber numberWithInt:serviceType] forKey:@"service-type"];
	
	NotifyEventListener(kSocialShareFinished, ToJsonCString(dataDict));
}

@end
