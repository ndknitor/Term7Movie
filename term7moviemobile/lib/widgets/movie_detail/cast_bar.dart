import 'package:flutter/material.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'caster_item.dart';

class CastBar extends StatelessWidget {
  const CastBar({
    Key? key,
    required this.size,
  }) : super(key: key);
  // final List<TicketStates> dateStates = [
  //   TicketStates.idle,
  //   TicketStates.active,
  //   TicketStates.busy,
  //   TicketStates.idle
  // ];
  // final List<TicketStates> timeStates_1 = [
  //   TicketStates.idle,
  //   TicketStates.idle,
  //   TicketStates.busy,
  //   TicketStates.idle
  // ];
  // final List<TicketStates> timeStates_2 = [
  //   TicketStates.idle,
  //   TicketStates.busy,
  //   TicketStates.active,
  //   TicketStates.idle
  // ];
  // final List<TicketStates> timeStates_3 = [
  //   TicketStates.idle,
  //   TicketStates.busy,
  //   TicketStates.idle,
  //   TicketStates.idle
  // ];
  // final List<TicketStates> timeStates_4 = [
  //   TicketStates.idle,
  //   TicketStates.busy,
  //   TicketStates.idle,
  //   TicketStates.idle
  // ];


  final Size size;

  @override
  Widget build(BuildContext context) {

    List<dynamic> casters = [
      {'name': 'Reilly', 'profileImageUrl': "https://www.cgv.vn/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/d/r/dr-strange-payoff-poster_1_.jpg"},
      {'name': 'Reilly', 'profileImageUrl': "https://www.cgv.vn/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/d/r/dr-strange-payoff-poster_1_.jpg"},
      {'name': 'Reilly', 'profileImageUrl': "https://www.cgv.vn/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/d/r/dr-strange-payoff-poster_1_.jpg"},
      {'name': 'Reilly', 'profileImageUrl': "https://www.cgv.vn/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/d/r/dr-strange-payoff-poster_1_.jpg"},
    ];

    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        children: casters
            .map((e) => Builder(builder: (context) {
                  return CasterItem(
                    size: size,
                    cast: e,
                  );
                }))
            .toList(),
      ),
    );
  }
}
