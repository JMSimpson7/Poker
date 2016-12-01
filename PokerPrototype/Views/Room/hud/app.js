var app = new Vue({	
	el: '#hud',

	data: {
		username: "username",
		card1Url: 'cardImages/h03.png',
		card2Url: 'cardImages/c01.png',
		avatarUrl: 'data:image/jpeg;base64,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',
		currency: 0.00,
		timer: 0
	},

	methods: {
		startClock: function(){
						this.timer = 30;
						var that = this;
						var clock = setInterval(countdown, 1000);
						function countdown (){
							that.timer--; 
							if(that.timer == 0){
								clearInterval(clock);
							}
						}
		}
	}
})
