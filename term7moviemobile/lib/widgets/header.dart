import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class HomeHeader extends StatelessWidget {
  const HomeHeader({
    Key? key,
    required this.size,
  }) : super(key: key);
  final Size size;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(top: 64, left: 24, right: 24),
      child: SizedBox(
        height: size.height / 10,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                ClipRRect(
                  borderRadius: BorderRadius.circular(30),
                  child: CachedNetworkImage(
                    fit: BoxFit.cover,
                    imageUrl: AuthController.instance.user!.photoURL ?? Constants.defaultAvatar,
                    height: 38,
                    width: 38,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(left: 8.0),
                  child: SizedBox(
                    width: 150,
                    child: Text(AuthController.instance.user!.displayName ?? "Name", style: TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.w600
                    ),),
                  ),
                ),
              ],
            ),

            IconButton(
              onPressed: () {},
              icon: SvgPicture.asset(
                "assets/images/notifications.svg",
                color: MyTheme.bottomBarColor
              ),
            ),
          ],
        ),
      ),
    );
  }
}
