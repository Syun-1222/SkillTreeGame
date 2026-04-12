; 2025/03/09 @ko10panda edit

;=======================================================================================================================
;
;	コンフィグ画面作成
;
;=======================================================================================================================

[mask time="100"]

[layopt layer="message0" visible="false"]
[clearfix]
[stop_keyconfig]
[free_layermode time="10" wait="true"]
[reset_camera time="10" wait="true"]
[hidemenubutton]

[iscript]

	$(".layer_camera").empty();
	$("#bgmovie").remove();

	TG.config.autoRecordLabel = "true";

/*
 * コンフィグで使用する変数
 *
 * tf.img_path            共通：画像類のパス
 * tf.uiConfig            コンフィグで使用する画像、サイズ、配置座標を管理するオブジェクト
 *
 * tf.current_bgm_vol     BGM音量：現在のBGM音量
 * tf.current_se_vol      SE音量：現在のSE音量
 * tf.current_ch_speed    テキスト速度：現在のテキスト速度
 * tf.current_auto_speed  オートウェイト：現在のオートウェイト
 *
 * tf.text_skip           未読スキップ：現在の未読スキップの状態
 * tf.screen_size         画面サイズ：現在の画面サイズ
 *
 * f.prev_vol_list        BGM、SE：BGMとSEの音量とインデックスを保存する配列
 *
*/

	tf.img_path = '../others/plugin/theme_kopanda_bth_13_dk/image/config/';

	tf.uiConfig = {

		img_btn : tf.img_path + 'c_btn.gif',

		mute : {
			img            : tf.img_path + 'mute_act.png',
			img_hov        : tf.img_path + 'mute_hov.png',
			width          : 78,
			height         : 78,
			pos_bgm        : [1557, 227],
			pos_se         : [1557, 323]
    },

		skip : {
			img_off        : tf.img_path + 'skip_off_act.png',
			img_off_hov    : tf.img_path + 'skip_hov.png',
			img_on         : tf.img_path + 'skip_on_act.png',
			img_on_hov     : tf.img_path + 'skip_hov.png',
			width          : 308,
			height         :  56,
			pos_off        : [ 868, 766],
			pos_on         : [1192, 766]
		},

		screen : {
			img_window     : tf.img_path + 'windowed_act.png',
			img_window_hov : tf.img_path + 'screen_hov.png',
			img_full       : tf.img_path + 'fullscreen_act.png',
			img_full_hov   : tf.img_path + 'screen_hov.png',
			width          : 308,
			height         :  56,
			pos_full       : [ 868, 862],
			pos_window     : [1192, 862]
		},

		slider : {
			slider_top     : [264, 358, 450, 544],
			slider_left    : 924,
			slider_width   : 480
		}

	};

	tf.current_bgm_vol    = parseInt(TG.config.defaultBgmVolume);
	tf.current_se_vol     = parseInt(TG.config.defaultSeVolume);
	tf.current_ch_speed   = parseInt(TG.config.chSpeed);
	tf.current_auto_speed = parseInt(TG.config.autoSpeed);

	tf.text_skip ="ON";
		if(TG.config.unReadTextSkip != "true") {
			tf.text_skip ="OFF";
		}

	tf.screen_size = (function() {
		if ((document.FullscreenElement !== undefined && document.FullscreenElement !== null) ||
				(document.webkitFullscreenElement !== undefined && document.webkitFullscreenElement !== null) ||
				(document.msFullscreenElement !== undefined && document.msFullscreenElement !== null)) {
				return 'full';
			} else {
				return 'window';
		}
	})();

	// ミュート解除時はここからミュート直前の音量設定を取得する
	if(typeof f.prev_vol_list === 'undefined') {
		f.prev_vol_list = [tf.current_bgm_vol, tf.current_se_vol];
	}

[endscript]

[cm]
;-----------------------------------------------------------------------------------------------------------------------
; 背景
;-----------------------------------------------------------------------------------------------------------------------
[bg storage="&tf.img_path +'config_bg.png'" time="100" wait="false"]
[image name="label_config anim_label_slide_nlp" storage="&tf.img_path +'label_config.png'" layer="0" x="0" y="0" visible="true" time="100" wait="false"]

;-----------------------------------------------------------------------------------------------------------------------
; 閉じるボタン
;-----------------------------------------------------------------------------------------------------------------------
[button name="back_btn" fix="true" graphic="&tf.img_path + 'btn_back.png'" enterimg="&tf.img_path + 'btn_back_hov.png'" activeimg="&tf.img_path + 'btn_back_clk.png'" target="*backtitle" x="1756" y="56"]

[jump target="*config_page"]

*config_page

[clearstack]

;-----------------------------------------------------------------------------------------------------------------------
; スライダー配置
;-----------------------------------------------------------------------------------------------------------------------
[iscript]

/* tooltips format */
const format = {
	to: function (value) {
		return Math.round(value).toLocaleString();
	},
	from: function (value) {
		return Number(value.replace(/,/g, ''));
	}
};

/* ---------------------------------------------------------------------------------------------------------------------

 BGM Vol

------------------------------------------------------------------------------------------------------------------------ */

	$(".layer_free").append('<div id="bgm_volume" class="slider"></div>');

	const bgm_vol_slider = document.getElementById('bgm_volume');

	// style
	bgm_vol_slider.style.top   = tf.uiConfig.slider.slider_top[0] + 'px';
	bgm_vol_slider.style.left  = tf.uiConfig.slider.slider_left + 'px';
	bgm_vol_slider.style.width = tf.uiConfig.slider.slider_width + 'px';

	// slider setting
	noUiSlider.create(bgm_vol_slider,{
		start: tf.current_bgm_vol,
		connect: 'lower',
		behaviour: 'tap-drag',
		step: 1,
		range:{
			'min': 0,
			'max': 100
		},
		tooltips: true,
		format: format
	});

// update
bgm_vol_slider.noUiSlider.on('update', function(values, handle){
	tf.current_bgm_vol = values[handle];
	TG.ftag.startTag("bgmopt", {volume: values[handle]});
	if(tf.current_bgm_vol == 0){
		$('.mute_bgm').show();
	} else {
		$('.mute_bgm').hide();
	};
});


/* ---------------------------------------------------------------------------------------------------------------------

 SE Vol

------------------------------------------------------------------------------------------------------------------------ */

	$(".layer_free").append('<div id="se_volume" class="slider"></div>');

	const se_vol_slider = document.getElementById('se_volume');

	// style
	se_vol_slider.style.top   = tf.uiConfig.slider.slider_top[1] + 'px';
	se_vol_slider.style.left  = tf.uiConfig.slider.slider_left + 'px';
	se_vol_slider.style.width = tf.uiConfig.slider.slider_width + 'px';

	// slider setting
	noUiSlider.create(se_vol_slider,{
		start: tf.current_se_vol,
		connect: 'lower',
		behaviour: 'tap-drag',
		step: 1,
		range:{
			'min': 0,
			'max': 100
		},
		tooltips: true,
		format: format
	});

	// update
	se_vol_slider.noUiSlider.on('update', function(values, handle){
		tf.current_se_vol = values[handle];
		TG.ftag.startTag("seopt", {volume: values[handle]});
		if(tf.current_se_vol == 0){
			$('.mute_se').show();
		} else {
			$('.mute_se').hide();
		};
	});

/* ---------------------------------------------------------------------------------------------------------------------

 TextSpeed

------------------------------------------------------------------------------------------------------------------------ */

	$(".layer_free").append('<div id="ch_speed" class="slider"></div>');

	const ch_speed_slider = document.getElementById('ch_speed');

	// style
	ch_speed_slider.style.top   = tf.uiConfig.slider.slider_top[2] + 'px';
	ch_speed_slider.style.left  = tf.uiConfig.slider.slider_left + 'px';
	ch_speed_slider.style.width = tf.uiConfig.slider.slider_width + 'px';

  // slider setting
	noUiSlider.create(ch_speed_slider,{
		start: tf.current_ch_speed,
		connect: 'upper',
		direction: 'rtl',
		behaviour: 'tap-drag',
		step: 1,
		range:{
			'min': 5,
			'max': 100
		},
		tooltips: true,
		format: wNumb({
			decimals: 0
		})
	});

	// update
	ch_speed_slider.noUiSlider.on('change', function(values, handle){
		tf.current_ch_speed = values[handle];
		TG.ftag.startTag("configdelay", {speed: values[handle]});
		gMessageTester.currentTextNumber = 0;
		gMessageTester.next(true);
	});

/* ---------------------------------------------------------------------------------------------------------------------

 AutoTextSpeed

------------------------------------------------------------------------------------------------------------------------ */

	$(".layer_free").append('<div id="auto_speed" class="slider"></div>');

	const auto_speed_slider = document.getElementById('auto_speed');

	// style
	auto_speed_slider.style.top   = tf.uiConfig.slider.slider_top[3] + 'px';
	auto_speed_slider.style.left  = tf.uiConfig.slider.slider_left + 'px';
	auto_speed_slider.style.width = tf.uiConfig.slider.slider_width + 'px';

	// slider setting
	noUiSlider.create(auto_speed_slider,{
		start: tf.current_auto_speed,
		connect: 'upper',
		direction: 'rtl',
		behaviour: 'tap-drag',
		step: 1,
		range:{
			'min': 500,
			'max': 5000
		},
		tooltips: true,
		format: wNumb({
			decimals: 0
		})
	});

// update
auto_speed_slider.noUiSlider.on('update', function(values, handle){
	tf.current_auto_speed = values[handle];
	TG.ftag.startTag("autoconfig", {speed: values[handle]});
});

[endscript]

;=======================================================================================================================

; Button

;=======================================================================================================================

; Mute BGM
[button fix="true" target="*vol_bgm_mute" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.mute.img_hov" width="&tf.uiConfig.mute.width" height="&tf.uiConfig.mute.height" x="&tf.uiConfig.mute.pos_bgm[0]" y="&tf.uiConfig.mute.pos_bgm[1]"]

; Mute SE
[button fix="true" target="*vol_se_mute" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.mute.img_hov" width="&tf.uiConfig.mute.width" height="&tf.uiConfig.mute.height" x="&tf.uiConfig.mute.pos_se[0]" y="&tf.uiConfig.mute.pos_se[1]"]

; Unread Text Skip -- Skip Off
[button name="unread_off" fix="true" target="*skip_off" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.skip.img_off_hov" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height" x="&tf.uiConfig.skip.pos_off[0]" y="&tf.uiConfig.skip.pos_off[1]"]

; Unread Text Skip -- Skip On
[button name="unread_on" fix="true" target="*skip_on" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.skip.img_on_hov" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height" x="&tf.uiConfig.skip.pos_on[0]" y="&tf.uiConfig.skip.pos_on[1]"]

; Screen Size -- FullScreen
[button name="screen_full" fix="true" target="*screen_full" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.screen.img_full_hov" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height" x="&tf.uiConfig.screen.pos_full[0]" y="&tf.uiConfig.screen.pos_full[1]"]

; Screen Size -- Windowed
[button name="screen_window" fix="true" target="*screen_window" graphic="&tf.uiConfig.img_btn" enterimg="&tf.uiConfig.screen.img_window_hov" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height" x="&tf.uiConfig.screen.pos_window[0]" y="&tf.uiConfig.screen.pos_window[1]"]

;-----------------------------------------------------------------------------------------------------------------------
; コンフィグ起動時に読み込み
;-----------------------------------------------------------------------------------------------------------------------
[layopt layer="0" visible="true"]

[call target="*load_bgm_img"]
[call target="*load_se_img"]
[call target="*load_skip_img"]
[call target="*load_screen_img"]

[test_message_start]

[mask_off time="300"]

[s]

;-----------------------------------------------------------------------------------------------------------------------
; コンフィグモード終了
;-----------------------------------------------------------------------------------------------------------------------
*backtitle
[mask time="250"]

[cm]
[layopt layer="message1" visible="false"]
[endkeyframe]
[freeimage layer="0"]
[freeimage layer="base"]
[clearfix]
[clearstack]
[start_keyconfig]

[iscript]
  $(".layer_free").empty();
[endscript]

[mask_off time="10"]

[awakegame]

;=======================================================================================================================

; ボタンクリック時の処理

;=======================================================================================================================
;-----------------------------------------------------------------------------------------------------------------------
; Mute BGM
;-----------------------------------------------------------------------------------------------------------------------
*vol_bgm_mute
[iscript]

const bgm_vol_slider = document.getElementById('bgm_volume');

// mute
if(tf.current_bgm_vol != 0){
	f.prev_vol_list[0] = tf.current_bgm_vol;
	tf.current_bgm_vol = 0;
	bgm_vol_slider.noUiSlider.set(0);
} else {
	tf.current_bgm_vol = f.prev_vol_list[0];
	bgm_vol_slider.noUiSlider.set(tf.current_bgm_vol);
}

// setting
TG.ftag.startTag("bgmopt", {volume: tf.current_bgm_vol});

[endscript]

; reload img
[call target="*load_bgm_img"]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Mute SE
;-----------------------------------------------------------------------------------------------------------------------
*vol_se_mute
[iscript]

const se_vol_slider = document.getElementById('se_volume');

// mute
if(tf.current_se_vol != 0){
	f.prev_vol_list[1] = tf.current_se_vol;
	tf.current_se_vol = 0;
	se_vol_slider.noUiSlider.set(0);
} else {
	tf.current_se_vol = f.prev_vol_list[1];
	se_vol_slider.noUiSlider.set(tf.current_se_vol);
}

// setting
TG.ftag.startTag("seopt", {volume: tf.current_se_vol});

[endscript]

; reload img
[call target="*load_se_img"]

[endscript]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Unread Text Skip - Off
;-----------------------------------------------------------------------------------------------------------------------
*skip_off
[free layer="0" name="unread_on" time="10"]
[image layer="0" name="unread_off" storage="&tf.uiConfig.skip.img_off" x="&tf.uiConfig.skip.pos_off[0]" y="&tf.uiConfig.skip.pos_off[1]" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height"]
[config_record_label skip="false"]

[return]

;-------------------------------------------------------------------------------
; Unread Text Skip - On
;-------------------------------------------------------------------------------
*skip_on
[free layer="0" name="unread_off" time="10"]
[image layer="0" name="unread_on" storage="&tf.uiConfig.skip.img_on" x="&tf.uiConfig.skip.pos_on[0]" y="&tf.uiConfig.skip.pos_on[1]" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height"]
[config_record_label skip="true"]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Screen Size - FullScreen
;-----------------------------------------------------------------------------------------------------------------------
*screen_full
[if exp="tf.screen_size == 'window'"]
	[screen_full]
	[free layer="0" name="screen_window" time="10"]
	[image layer="0" name="screen_full" storage="&tf.uiConfig.screen.img_window" x="&tf.uiConfig.screen.pos_full[0]" y="&tf.uiConfig.screen.pos_full[1]" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height"]
	[eval exp="tf.screen_size = 'full'"]
[endif]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Screen Size - Windowed
;-----------------------------------------------------------------------------------------------------------------------
*screen_window
[if exp="tf.screen_size == 'full'"]
	[screen_full]
	[free layer="0" name="screen_full" time="10"]
	[image layer="0" name="screen_window" storage="&tf.uiConfig.screen.img_full" x="&tf.uiConfig.screen.pos_window[0]" y="&tf.uiConfig.screen.pos_window[1]" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height"]
	[eval exp="tf.screen_size = 'window'"]
[endif]

[return]

;===============================================================================

; reload img

;===============================================================================
;-----------------------------------------------------------------------------------------------------------------------
; BGM Volume
;-----------------------------------------------------------------------------------------------------------------------
*load_bgm_img

[image layer="0" name="mute_bgm" storage="&tf.uiConfig.mute.img" x="&tf.uiConfig.mute.pos_bgm[0]" y="&tf.uiConfig.mute.pos_bgm[1]" width="&tf.uiConfig.mute.width" height="&tf.uiConfig.mute.height" time="10" visible="false"]

[iscript]
// 音量が0のときはミュートにチェックを入れる
if(tf.current_bgm_vol == 0){
	$('.mute_bgm').show();
} else {
	$('.mute_bgm').hide();
}
[endscript]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; SE Volume
;-----------------------------------------------------------------------------------------------------------------------
*load_se_img

[image layer="0" name="mute_se" storage="&tf.uiConfig.mute.img" x="&tf.uiConfig.mute.pos_se[0]" y="&tf.uiConfig.mute.pos_se[1]" width="&tf.uiConfig.mute.width" height="&tf.uiConfig.mute.height" time="10" visible="false"]

[iscript]
// 音量が0のときはミュートにチェックを入れる
if(tf.current_se_vol == 0){
	$('.mute_se').show();
} else {
	$('.mute_se').hide();
}
[endscript]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Unread Text Skip
;-----------------------------------------------------------------------------------------------------------------------
*load_skip_img

[if exp="tf.text_skip == 'ON'"]
	[image layer="0" name="unread_on" storage="&tf.uiConfig.skip.img_on" x="&tf.uiConfig.skip.pos_on[0]" y="&tf.uiConfig.skip.pos_on[1]" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height"]
[else]
	[image layer="0" name="unread_off" storage="&tf.uiConfig.skip.img_off" x="&tf.uiConfig.skip.pos_off[0]" y="&tf.uiConfig.skip.pos_off[1]" width="&tf.uiConfig.skip.width" height="&tf.uiConfig.skip.height"]
[endif]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; Screen Size
;-----------------------------------------------------------------------------------------------------------------------
*load_screen_img
[if exp="tf.screen_size == 'full'"]
	[image layer="0" name="screen_full" storage="&tf.uiConfig.screen.img_full" x="&tf.uiConfig.screen.pos_full[0]" y="&tf.uiConfig.screen.pos_full[1]" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height"]
[else]
	[image layer="0" name="screen_window" storage="&tf.uiConfig.screen.img_window" x="&tf.uiConfig.screen.pos_window[0]" y="&tf.uiConfig.screen.pos_window[1]" width="&tf.uiConfig.screen.width" height="&tf.uiConfig.screen.height"]
[endif]

[return]

;-----------------------------------------------------------------------------------------------------------------------
; textmessage
;-----------------------------------------------------------------------------------------------------------------------
*messagetest
[test_message_reset]
[return]
