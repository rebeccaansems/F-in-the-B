//
//  NPSharingHandler.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 05/01/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPSharingHandler.h"
#import "WhatsAppActivity.h"
#import "UIHandler.h"
#import "CustomActivityViewController.h"

@implementation NPSharingHandler

#define kSharingFinished	"SharingFinished"

@synthesize popoverController;
@synthesize mailShare;
@synthesize messagingShare;
@synthesize whatsAppShare;
@synthesize socialShare;

#pragma mark - Static methods

+ (MailShare *)MailShare
{
	return [[NPSharingHandler Instance] mailShare];
}

+ (MessagingShare *)MessagingShare
{
	return [[NPSharingHandler Instance] messagingShare];
}

+ (WhatsAppShare *)WhatsAppShare
{
	return [[NPSharingHandler Instance] whatsAppShare];
}

+ (SocialShare *)SocialShare
{
	return [[NPSharingHandler Instance] socialShare];
}

#pragma mark - Init

- (id)init
{
	self	= [super init];
	
	if (self)
	{
		self.mailShare		= [[[MailShare alloc] init] autorelease];
		self.messagingShare	= [[[MessagingShare alloc] init] autorelease];
		self.whatsAppShare	= [[[WhatsAppShare alloc] init] autorelease];
		self.socialShare	= [[[SocialShare alloc] init] autorelease];
	}
	
	return self;
}

- (void)dealloc
{
    self.popoverController  = NULL;
    self.mailShare			= NULL;
	self.messagingShare		= NULL;
	self.whatsAppShare		= NULL;
	self.socialShare		= NULL;
	
    [super dealloc];
}

#pragma mark - Share sheet

- (void)shareMessage:(NSString *)message
                 URL:(NSString *)URLString
            andImage:(UIImage *)image
 withExcludedSharing:(NSArray *)excludedOptions
{
	NSMutableArray *objectsToShare  = [NSMutableArray array];
	
	if (!IsNullOrEmpty(message))
		[objectsToShare addObject:message];
	
    if (!IsNullOrEmpty(URLString))
        [objectsToShare addObject:[NSURL URLWithString:URLString]];
    
    if (image != NULL)
		[objectsToShare addObject:image];
    
    // Set excluded options, by default few ios 6+ activities are added
    NSMutableArray *excludedActivities      = [NSMutableArray arrayWithObjects:
											   UIActivityTypePostToWeibo,
                                               UIActivityTypePrint,
                                               UIActivityTypeCopyToPasteboard,
                                               UIActivityTypeAssignToContact,
                                               UIActivityTypeSaveToCameraRoll,
											   nil];
    
    // Activities supported in ios7 +
#ifdef __IPHONE_7_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
	{
		[excludedActivities addObjectsFromArray:@[UIActivityTypeAddToReadingList,
												  UIActivityTypePostToFlickr,
												  UIActivityTypePostToVimeo,
												  UIActivityTypePostToTencentWeibo,
												  UIActivityTypeAirDrop]];
	}
#endif
	
    for (NSNumber *option in excludedOptions)
	{
		NSString *excludedActivity	= [self getAcivityTypeForShareOption:[option intValue]];
		
		// Add it to the exclusion list
		[excludedActivities addObject:excludedActivity];
	}
    
	
	CustomActivityViewController *activityVC    = [[[CustomActivityViewController alloc] initWithActivityItems:objectsToShare
																						 applicationActivities:[self getApplicationActivities]] autorelease];
	activityVC.excludedActivityTypes    		= excludedActivities;

    // Set completion handler
#ifdef __IPHONE_8_0
	if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"8.0"))
	{
		[activityVC setCompletionWithItemsHandler:^(NSString *activityType, BOOL completed, NSArray *returnedItems, NSError *activityError){
			
			// Unset completion handler
			[activityVC setCompletionWithItemsHandler:nil];
			
			// Invoke handler
			[self activityViewControllerWithActivityType:activityType
									 didFinishWithStatus:completed];
		}];
	}
	else
#endif
	{
		[activityVC setCompletionHandler:^(NSString *activityType, BOOL completed){
			
			// Unset completion handler
			[activityVC setCompletionHandler:nil];
			
			// Invoke handler
			[self activityViewControllerWithActivityType:activityType
									 didFinishWithStatus:completed];
		}];
	}
	
    // Present it
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
    {
        self.popoverController  = [[[UIPopoverController alloc] initWithContentViewController:activityVC] autorelease];

        CGRect popoverRect;
		popoverRect.origin		= [[UIHandler Instance] popoverPoint];
		popoverRect.size		= CGSizeMake(1, 1);
        
        [self.popoverController presentPopoverFromRect:popoverRect
                                                inView:UnityGetGLView()
                              permittedArrowDirections:UIPopoverArrowDirectionAny
                                              animated:YES];
    }
    else
    {
        [UnityGetGLViewController() presentViewController:activityVC
												 animated:YES
											   completion:nil];
    }
}

- (void)activityViewControllerWithActivityType:(NSString *)activityType didFinishWithStatus:(BOOL)completed
{
	NSLog(@"[SharingHandler] dismissed sharing, selected activity: %@  action is completed: %d", activityType, completed);
	
	if (completed)
	{
		NotifyEventListener(kSharingFinished, "true");
	}
	else
	{
		NotifyEventListener(kSharingFinished, "false");
	}
	
	// Dismiss controller
	if (self.popoverController)
	{
		[self.popoverController dismissPopoverAnimated:YES];
		self.popoverController = NULL;
	}
	else
	{
		[UnityGetGLViewController() dismissViewControllerAnimated:YES
													   completion:NULL];
	}
}

#pragma mark - Misc

- (NSString *)getAcivityTypeForShareOption:(eShareOptions)shareOption
{
    switch (shareOption)
    {
        case MESSAGE:
            return UIActivityTypeMessage;
            
        case MAIL:
            return UIActivityTypeMail;
            
        case FB:
            return UIActivityTypePostToFacebook;
            
        case TWITTER:
            return UIActivityTypePostToTwitter;
            
        case WHATSAPP:
            return UIActivityTypePostToWhatsApp;
            
        default:
            break;
    }
    
    return @"";
}

- (eShareOptions)getShareOptionForActivityType:(NSString *)type
{
    if ([type isEqualToString:UIActivityTypeMessage])
        return MESSAGE;
    else if ([type isEqualToString:UIActivityTypeMail])
        return MAIL;
    else if ([type isEqualToString:UIActivityTypePostToFacebook])
        return FB;
    else if ([type isEqualToString:UIActivityTypePostToTwitter])
        return TWITTER;
    else if ([type isEqualToString:UIActivityTypePostToWhatsApp])
        return WHATSAPP;
    
    return UNDEFINED;
}

- (NSArray *)getApplicationActivities
{
	if (SYSTEM_VERSION_LESS_THAN_OR_EQUAL_TO(@"8.0"))
		return @[[[[WhatsAppActivity alloc] init] autorelease]];

	return NULL;
}

@end