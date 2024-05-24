import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Drawer from '@material-ui/core/Drawer';
import { IconMenu2 } from '@tabler/icons-react';
import IconButton from '@material-ui/core/IconButton';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import { useState } from 'react';
import { Link } from 'react-router-dom';

const useStyles = makeStyles({
	drawer: {
		width: 250,
	},
});

export const Header = () => {
	const [isDrawerOpen, setIsDrawerOpen] = useState(false);
	const classes = useStyles();

	return (
		<AppBar variant="outlined" position="static">
			<Toolbar>
				<IconButton
					edge="start"
					color="inherit"
					aria-label="menu"
					onClick={() => setIsDrawerOpen(true)}
				>
					<IconMenu2 />
				</IconButton>
				<Typography variant="h6">eShopOnTelegram</Typography>

				<Drawer
					open={isDrawerOpen}
					onClose={() => setIsDrawerOpen(false)}
				>
					<List className={classes.drawer}>
						<ListItem button>
							<Link to={'products'}>
								<ListItemText
									primary="Products"
									onClick={() => setIsDrawerOpen(false)}
								/>
							</Link>
						</ListItem>

						<ListItem button>
							<Link to={'productCategories'}>
								<ListItemText
									primary="Product categories"
									onClick={() => setIsDrawerOpen(false)}
								/>
							</Link>
						</ListItem>
					</List>
				</Drawer>
			</Toolbar>
		</AppBar>
	);
};
